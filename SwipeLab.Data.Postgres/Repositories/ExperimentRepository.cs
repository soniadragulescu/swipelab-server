using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SwipeLab.Data.Postgres.Configuration;
using SwipeLab.Domain.Experiment;
using SwipeLab.Domain.Experiment.Enums;

namespace SwipeLab.Data.Postgres.Repositories;

public class ExperimentRepository : IExperimentRepository
{
    private readonly ILogger<ExperimentRepository> _logger;
    private readonly ApplicationDbContext _context;

    public ExperimentRepository(ILogger<ExperimentRepository> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task CreateExperiment(Experiment experiment)
    {
        _logger.LogInformation("Creating experiment");

        var entity = Entities.Experiment.FromDomain(experiment);

        _context.Add(entity);

        await _context.SaveChangesAsync();

        _logger.LogInformation("Experiment created successfully with Id: {ExperimentId}",
            entity.ExperimentId);

        experiment.ExperimentId = entity.ExperimentId;
    }

    public async Task UpdateExperiment(Experiment experiment)
    {
        _logger.LogInformation("Updating experiment with Id: {ExperimentId} to state {state}", experiment.ExperimentId, experiment.State.ToString());

        await _context.Experiments.Where(e => e.ExperimentId == experiment.ExperimentId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(e => e.State, experiment.State)
                .SetProperty(e => e.RowLastUpdated, DateTime.UtcNow)
            );

        _logger.LogInformation("Experiment with Id: {ExperimentId} updated with new state: {State}", experiment.ExperimentId, experiment.State.ToString());
    }

    public async Task<Experiment?> GetExperiment(Guid experimentId)
    {
        _logger.LogInformation("Fetching experiment with Id: {ExperimentId}", experimentId);

        var experiment = await _context.Experiments
            .AsNoTracking()
            .Where(e => e.ExperimentId == experimentId)
            .Select(e => Entities.Experiment.ToDomain(e))
            .FirstOrDefaultAsync();

        if (experiment == null)
        {
            return null;
        }

        await EnhanceExperimentWithCounts(experiment);

        return experiment;
    }

    private async Task EnhanceExperimentWithCounts(Experiment experiment)
    {
        experiment.SwipeCount = await _context.DatingProfileSwipes
            .CountAsync(x => x.DatingProfile != null &&
                x.DatingProfile.DatingProfileSetId == experiment.DatingProfileSetId);
        experiment.ReflectionCount = await _context.DatingProfileReflections
            .CountAsync(x => x.DatingProfile != null
                && x.DatingProfile.DatingProfileSetId == experiment.DatingProfileSetId);
    }

    public async Task<Experiment?> GetExperimentByDatingProfileSetId(Guid datingProfileSetId)
    {
        _logger.LogInformation("Fetching experiment by datingProfileSetId: {DatingProfileSetId}", datingProfileSetId);

        var experiment = await _context.Experiments
            .AsNoTracking()
            .Where(e => e.DatingProfileSetId == datingProfileSetId)
            .Select(e => Entities.Experiment.ToDomain(e))
            .FirstOrDefaultAsync();

        if (experiment == null)
        {
            return null;
        }

        await EnhanceExperimentWithCounts(experiment);

        return experiment;
    }

    public async Task<Dictionary<ExperimentType, int>> GetCompletedExperimentsDistribution()
    {
        var completedCounts = await _context.Experiments
            .Where(e => e.State == ExperimentState.COMPLETE)
            .GroupBy(e => e.Type)
            .Select(g => new
            {
                ExperimentType = g.Key,
                Count = g.Count()
            })
            .ToListAsync();

        var allTypes = Enum.GetValues(typeof(ExperimentType)).Cast<ExperimentType>();

        var result = allTypes.ToDictionary(
            type => type,
            type => completedCounts.FirstOrDefault(x => x.ExperimentType == type)?.Count ?? 0
        );

        return result;
    }
}