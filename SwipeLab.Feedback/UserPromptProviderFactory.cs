using Microsoft.Extensions.DependencyInjection;
using SwipeLab.Domain.Experiment.Enums;
using SwipeLab.Feedback.LargeLanguageModels.UserPromptProviders;

namespace SwipeLab.Feedback;

public class UserPromptProviderFactory : IUserPromptProviderFactory
{
    private readonly IServiceProvider _serviceProvider;

    public UserPromptProviderFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IUserPromptProvider GetUserPromptProvider(ExperimentType experimentType)
    {
        return experimentType switch
        {
            ExperimentType.PREDEFINED_PROMPTS => _serviceProvider.GetRequiredService<PredefinedUserPromptProvider>(),
            ExperimentType.GEMINI_PROMTS => _serviceProvider.GetRequiredService<GeminiUserPromptProvider>(),
            _ => throw new NotImplementedException($"No provider for experiment type {experimentType.ToString()}")
        };
    }
}