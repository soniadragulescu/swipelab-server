using System.ComponentModel.DataAnnotations;
using SwipeLab.Data.Postgres.Models.Entities;
using SwipeLab.Domain.Experiment.Enums;

namespace SwipeLab.Data.Postgres.Entities;

public class Experiment : BaseEntity
{
    [Key] public Guid ExperimentId { get; set; }
    public Guid ParticipantId { get; set; }
    public Guid DatingProfileSetId { get; set; }
    public ExperimentState State { get; set; } = ExperimentState.SWIPING;
    public ExperimentType Type { get; set; } = ExperimentType.PREDEFINED_PROMPTS;

    public static Domain.Experiment.Experiment ToDomain(Experiment entity)
    {
        return new Domain.Experiment.Experiment()
        {
            ExperimentId = entity.ExperimentId,
            ParticipantId = entity.ParticipantId,
            DatingProfileSetId = entity.DatingProfileSetId,
            State = entity.State,
            Type = entity.Type,
        };
    }

    public static Experiment FromDomain(Domain.Experiment.Experiment domain)
    {
        return new Experiment()
        {
            ExperimentId = domain.ExperimentId == Guid.Empty ? Guid.NewGuid() : domain.ExperimentId,
            ParticipantId = domain.ParticipantId,
            DatingProfileSetId = domain.DatingProfileSetId,
            State = domain.State,
            Type = domain.Type,
        };
    }
}