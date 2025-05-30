using SwipeLab.Model.Responses.Feedback;

namespace SwipeLab.Services.Interfaces.Interfaces;

public interface IPromptService
{
    Task<SwipingFeedbackPrompts> GetPromptsForExperimentAsync(Guid experimentId);
}