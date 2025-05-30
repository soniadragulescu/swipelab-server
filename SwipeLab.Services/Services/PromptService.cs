using Microsoft.Extensions.Logging;
using SwipeLab.Feedback;
using SwipeLab.Model.Responses.Feedback;
using SwipeLab.Services.Interfaces.Interfaces;

namespace SwipeLab.Services.Services;

public class PromptService : IPromptService
{
    private readonly ILogger<PromptService> _logger;

    private readonly IUserPromptProviderFactory _userPromptProviderFactory;
    private readonly IExperimentService _experimentService;

    public PromptService(ILogger<PromptService> logger,
        IUserPromptProviderFactory userPromptProviderFactory,
        IExperimentService experimentService)
    {
        _logger = logger;
        _userPromptProviderFactory = userPromptProviderFactory;
        _experimentService = experimentService;
    }

    public async Task<SwipingFeedbackPrompts> GetPromptsForExperimentAsync(Guid experimentId)
    {
        var experiment = await _experimentService.GetExperiment(experimentId);

        if (experiment == null)
        {
            throw new ApplicationException($"Experiment {experimentId} not found");
        }

        var experimentInteractionData = await _experimentService.GetExperimentInteractionDataByProfileSetId(experiment.DatingProfileSetId);

        var previouseSwipeState = experimentInteractionData.DatingProfileInteractions.Any()
            ? experimentInteractionData.DatingProfileInteractions.Last().SwipeInformation.SwipeState
            : Domain.DatingProfileFeedback.SwipeState.Liked;

        var userPromptProvider = _userPromptProviderFactory.GetUserPromptProvider(experiment.Type);
        var result = await userPromptProvider.GetUserReflectiveQuestionsAsync(experimentInteractionData);

        _logger.LogInformation("Created user prompts: {@UserPrompts} for profile previously {previousSwipeState}", result, previouseSwipeState.ToString());

        return new SwipingFeedbackPrompts
        {
            Prompts = result,
            PreviousSwipeState = previouseSwipeState
        };
    }
}