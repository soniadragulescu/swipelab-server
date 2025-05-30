using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SwipeLab.Data.Postgres.Repositories;

namespace SwipeLab.Data.Postgres.Configuration;

public static class DependencyInjection
{
    public static void AddSwipeLabDbContext(this IServiceCollection services, string? connectionString)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));
    }

    public static void AddSwipeLabRepositories(this IServiceCollection services)
    {
        services.AddScoped<IExperimentRepository, ExperimentRepository>();
        services.AddScoped<IParticipantRepository, ParticipantRespository>();
        services.AddScoped<IDatingProfileSetRepository, DatingProfileSetRepository>();
        services.AddScoped<IDatingProfileRepository, DatingProfileRepository>();
        services.AddScoped<IDatingProfileFeedbackRepository, DatingProfileFeedbackRepository>();
        services.AddScoped<IQuestionAnswersRepository, QuestionAnswersRepository>();
    }

    public static void RunMigrations(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var scopeProvider = scope.ServiceProvider;

        var context = scopeProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
    }
}