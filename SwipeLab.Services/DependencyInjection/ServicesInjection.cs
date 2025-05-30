using Microsoft.Extensions.DependencyInjection;
using SwipeLab.Feedback.Configuration;
using SwipeLab.Services.Interfaces.Interfaces;
using SwipeLab.Services.Services;

namespace SwipeLab.Services.DependencyInjection
{
    public static class ServicesInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddHttpClient();

            services.AddFeedbackServices();

            services.AddScoped<IExperimentService, ExperimentService>();
            services.AddScoped<IProfilePictureService, ProfilePictureService>();
            services.AddScoped<IDatingProfileGenerationService, DatingProfileGenerationService>();
            services.AddScoped<IPromptService, PromptService>();
            services.AddScoped<IDatingProfileFeedbackService, DatingProfileFeedbackService>();

            return services;
        }
    }
}