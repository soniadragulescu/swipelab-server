using Microsoft.Extensions.DependencyInjection;
using SwipeLab.Feedback.LargeLanguageModels;
using SwipeLab.Feedback.LargeLanguageModels.UserPromptProviders;

namespace SwipeLab.Feedback.Configuration
{
    public static class ServicesInjection
    {
        public static IServiceCollection AddFeedbackServices(this IServiceCollection services)
        {
            services.AddScoped<IUserPromptProviderFactory, UserPromptProviderFactory>();
            services.AddScoped<ILlmPromptBuilder, LlmPromptBuilder>();

            services.AddScoped<PredefinedUserPromptProvider>();
            services.AddScoped<GeminiUserPromptProvider>();

            return services;
        }
    }
}
