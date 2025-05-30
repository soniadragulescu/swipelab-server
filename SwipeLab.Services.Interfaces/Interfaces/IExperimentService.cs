using SwipeLab.Domain.Experiment;
using SwipeLab.Domain.Participant;
using SwipeLab.Domain.Question;

namespace SwipeLab.Services.Interfaces.Interfaces;

public interface IExperimentService
{
    Task<Experiment> CreateExperiment(Participant participant, string onboardingConfidence, string onboardingComfortable);
    Task<Experiment?> GetExperiment(Guid experimentId);
    Task<ExperimentInteractionData> GetExperimentInteractionDataByProfileSetId(Guid experimentId);
    Task<bool> UpdateExperimentStateIfNeeded(Guid datingProfileId);
    Task<bool> CompleteExperimentAsync(Guid experimentId, List<QuestionAnswer> answers);
}