using SwipeLab.Domain.Experiment;
using SwipeLab.Domain.Experiment.Enums;

namespace SwipeLab.Data;

public interface IExperimentRepository
{
    Task CreateExperiment(Experiment experiment);
    Task UpdateExperiment(Experiment experiment);
    Task<Experiment?> GetExperiment(Guid experimentId);
    Task<Experiment?> GetExperimentByDatingProfileSetId(Guid datingProfileSetId);
    Task<Dictionary<ExperimentType, int>> GetCompletedExperimentsDistribution();
}