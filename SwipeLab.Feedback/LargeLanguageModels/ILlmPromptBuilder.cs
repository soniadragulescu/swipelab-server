using SwipeLab.Domain.Experiment;

namespace SwipeLab.Feedback.LargeLanguageModels;

public interface ILlmPromptBuilder
{
    string BuildReflectiveQuestionsPrompt(ExperimentInteractionData experimentInteractionData);
}