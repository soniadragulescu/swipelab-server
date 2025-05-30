using SwipeLab.Domain.Experiment;

namespace SwipeLab.Feedback;

public interface IUserPromptProvider
{
    Task<List<string>> GetUserReflectiveQuestionsAsync(ExperimentInteractionData experimentInteractionData);
}