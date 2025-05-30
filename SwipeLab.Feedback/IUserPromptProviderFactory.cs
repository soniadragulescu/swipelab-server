using SwipeLab.Domain.Experiment.Enums;

namespace SwipeLab.Feedback;

public interface IUserPromptProviderFactory
{
    IUserPromptProvider GetUserPromptProvider(ExperimentType experimentType);
}