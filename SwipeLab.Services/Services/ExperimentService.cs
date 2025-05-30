using Microsoft.Extensions.Logging;
using SwipeLab.Data;
using SwipeLab.Domain.DatingProfile;
using SwipeLab.Domain.DatingProfile.Constants;
using SwipeLab.Domain.DatingProfile.Enums;
using SwipeLab.Domain.DatingProfile.Utils;
using SwipeLab.Domain.DatingProfileFeedback;
using SwipeLab.Domain.Enums;
using SwipeLab.Domain.Experiment;
using SwipeLab.Domain.Experiment.Enums;
using SwipeLab.Domain.Participant;
using SwipeLab.Domain.Question;
using SwipeLab.Services.Interfaces.Interfaces;

namespace SwipeLab.Services.Services;

public class ExperimentService : IExperimentService
{
    private readonly ILogger<ExperimentService> _logger;

    private readonly IExperimentRepository _experimentRepository;
    private readonly IParticipantRepository _participantRepository;
    private readonly IDatingProfileSetRepository _datingProfileSetRepository;
    private readonly IDatingProfileRepository _datingProfileRepository;
    private readonly IDatingProfileFeedbackRepository _datingProfileFeedbackRepository;
    private readonly IQuestionAnswersRepository _questionAnswersRepository;

    private readonly IDatingProfileGenerationService _datingProfileGenerationService;

    public ExperimentService(ILogger<ExperimentService> logger,
        IExperimentRepository experimentRepository,
        IParticipantRepository participantRepository,
        IDatingProfileSetRepository datingProfileSetRepository,
        IDatingProfileRepository datingProfileRepository,
        IDatingProfileGenerationService datingProfileGenerationService,
        IDatingProfileFeedbackRepository datingProfileFeedbackRepository,
        IQuestionAnswersRepository questionAnswersRepository)
    {
        _logger = logger;

        _experimentRepository = experimentRepository;
        _participantRepository = participantRepository;
        _datingProfileSetRepository = datingProfileSetRepository;
        _datingProfileRepository = datingProfileRepository;
        _datingProfileFeedbackRepository = datingProfileFeedbackRepository;
        _questionAnswersRepository = questionAnswersRepository;

        _datingProfileGenerationService = datingProfileGenerationService;
    }

    public async Task<Experiment> CreateExperiment(Participant participant, string onboardingConfidence, string onboardingComfortable)
    {
        _logger.LogInformation("Fetching experiment distribution...");

        var experimentDistribution = await _experimentRepository.GetCompletedExperimentsDistribution();
        var leastCompletedTypeEntry = experimentDistribution.Count > 0
            ? experimentDistribution.Aggregate((x, y) => x.Value < y.Value ? x : y)
            : new KeyValuePair<ExperimentType, int>(ExperimentType.PREDEFINED_PROMPTS, 0);

        _logger.LogInformation("Least completed experiment type is {ExperimentType} with {ExperimentCount} completed experiments", leastCompletedTypeEntry.Key, leastCompletedTypeEntry.Value);

        var experiment = new Experiment
        {
            Type = leastCompletedTypeEntry.Key
        };

        var participantId = await _participantRepository.CreateParticipant(participant);

        _logger.LogInformation("Created participant with Id {ParticipantId}", participantId);

        experiment.ParticipantId = participantId;

        var datingProfileSetConstraints = new DatingProfileSetGenerationConstraints()
        {
            SetSize = DatingProfileSetGenerationConstants.SET_SIZE,
            FixedGender = participant.InterestedIn == Gender.Unspecified ? null : participant.InterestedIn,
            EthnicityCounts = DistributionMapUtils.CalculateDistribution(DatingProfileSetGenerationConstants.SET_SIZE, new Dictionary<Ethnicity, double>()
            {
                { Ethnicity.Asian , DatingProfileSetGenerationConstants.ETHNICITY_DISTRIBUTION},
                { Ethnicity.Black , DatingProfileSetGenerationConstants.ETHNICITY_DISTRIBUTION},
                { Ethnicity.Latino , DatingProfileSetGenerationConstants.ETHNICITY_DISTRIBUTION},
                { Ethnicity.White , DatingProfileSetGenerationConstants.ETHNICITY_DISTRIBUTION}
            }),
            GenderCounts = participant.InterestedIn != Gender.Unspecified ? null : DistributionMapUtils.CalculateDistribution(DatingProfileSetGenerationConstants.SET_SIZE, new Dictionary<Gender, double>()
            {
                { Gender.Male, DatingProfileSetGenerationConstants.GENDER_DISTRIBUTION },
                { Gender.Female, DatingProfileSetGenerationConstants.GENDER_DISTRIBUTION },
            }),
            AgeRange = new AgeRange(participant.MinAge, participant.MaxAge)
        };

        var datingProfileSet = await _datingProfileGenerationService.GenerateDatingProfileSetAsync(datingProfileSetConstraints);

        var datingProfileSetId = await _datingProfileSetRepository.SaveDatingProfileSet(datingProfileSet);

        experiment.DatingProfileSetId = datingProfileSetId;

        await _experimentRepository.CreateExperiment(experiment);

        _logger.LogInformation("Created experiment with Id {ExperimentId}", experiment.ExperimentId);
        _logger.LogInformation("Saving initial onboarding answers...");

        var initialOnBoardingAnswers = new List<QuestionAnswer>
        {
            new QuestionAnswer
            {
                QuestionNumber = 99,
                Text = "One a scale of 1-7, how CONFIDENT do you think you'd feel deciding whether to like or reject a profile?",
                Answer = onboardingConfidence,
                ExperimentId = experiment.ExperimentId
            },
            new QuestionAnswer
            {
                QuestionNumber = 100,
                Text = "One a scale of 1-7, how COMFORTABLE do you think you'd feel deciding whether to like or reject a profile?",
                Answer = onboardingComfortable,
                ExperimentId = experiment.ExperimentId
            }
        };

        await _questionAnswersRepository.SaveQuestionAnswers(initialOnBoardingAnswers);

        _logger.LogInformation("Initial onboarding answers saved");

        return experiment;
    }

    public async Task<Experiment?> GetExperiment(Guid experimentId)
    {
        var experiment = await _experimentRepository.GetExperiment(experimentId);

        if (experiment == null)
        {
            return null;
        }

        return experiment;
    }

    public async Task<ExperimentInteractionData> GetExperimentInteractionDataByProfileSetId(Guid datingProfileSetId)
    {

        _logger.LogInformation("Fetching dating profile set {DatingProfileSetId}", datingProfileSetId);

        var datingProfileSet = await _datingProfileSetRepository.GetDatingProfileSetById(datingProfileSetId);

        if (datingProfileSet == null || datingProfileSet.DatingProfiles == null)
        {
            _logger.LogWarning("Dating profile set with Id {DatingProfileSetId} not found", datingProfileSetId);
            throw new ArgumentException($"DatingProfileSet {datingProfileSetId} not found");
        }

        _logger.LogInformation("Dating profile set found: {@DatingProfileSet}", datingProfileSet);

        return await GetInteractionDataForProfiles(datingProfileSet.DatingProfiles);
    }

    public async Task<bool> UpdateExperimentStateIfNeeded(Guid datingProfileId)
    {
        var datingProfile = await _datingProfileRepository.GetDatingProfile(datingProfileId);
        if (datingProfile == null || datingProfile.DatingProfileSetId == null)
        {
            _logger.LogError("Dating profile with Id {DatingProfileId} not found", datingProfileId);
            return false;
        }
        var datingProfileSet = await _datingProfileSetRepository.GetDatingProfileSetById((Guid)datingProfile.DatingProfileSetId);

        if (datingProfileSet == null || datingProfileSet.DatingProfiles == null)
        {
            _logger.LogError("Dating profile set with Id {DatingProfileSetId} not found", datingProfileSet?.DatingProfileSetId);
            return false;
        }

        var experiment = await _experimentRepository.GetExperimentByDatingProfileSetId(datingProfileSet.DatingProfileSetId);

        if (experiment == null)
        {
            _logger.LogError("Experiment with Id {ExperimentId} not found", datingProfileSet.DatingProfileSetId);
            return false;
        }

        if (experiment.SwipeCount >= datingProfileSet.DatingProfiles.Count)
        {
            _logger.LogInformation("The participant has swiped through all profiles.");

            if (experiment.ReflectionCount >= experiment.SwipeCount / 2)
            {
                _logger.LogInformation("The participant reflected on the second half of the profile set. Experiment with Id {ExperimentId} is complete", experiment.ExperimentId);

                experiment.State = ExperimentState.FINAL_FORM;
                await _experimentRepository.UpdateExperiment(experiment);

                return true;
            }
        }

        return false;
    }

    public async Task<bool> CompleteExperimentAsync(Guid experimentId, List<QuestionAnswer> answers)
    {
        var experiment = await _experimentRepository.GetExperiment(experimentId);

        if (experiment == null)
        {
            _logger.LogError("Experiment with Id {ExperimentId} not found", experimentId);

            return false;
        }

        await _questionAnswersRepository.SaveQuestionAnswers(answers);

        experiment.State = ExperimentState.COMPLETE;
        await _experimentRepository.UpdateExperiment(experiment);

        return true;
    }

    private async Task<ExperimentInteractionData> GetInteractionDataForProfiles(List<DatingProfile> datingProfiles)
    {
        var datingProfileInteractionDataList = new List<DatingProfileInteractionData>();

        foreach (var datingProfile in datingProfiles)
        {
            var swipeInformation =
                await _datingProfileFeedbackRepository.GetDatingProfileSwipeByDatingProfileId(datingProfile
                    .DatingProfileId);

            if (swipeInformation == null)
            {
                continue;
            }

            var userReflection =
                await _datingProfileFeedbackRepository.GetDatingProfileReflectionByDatingProfileId(datingProfile
                    .DatingProfileId);

            var interactionData = new DatingProfileInteractionData(datingProfile, swipeInformation, userReflection);

            datingProfileInteractionDataList.Add(interactionData);
        }

        var experimentInteractionData = new ExperimentInteractionData()
        {
            TotalProfileCount = datingProfiles.Count,
            DatingProfileInteractions = datingProfileInteractionDataList,
        };

        return experimentInteractionData;
    }
}