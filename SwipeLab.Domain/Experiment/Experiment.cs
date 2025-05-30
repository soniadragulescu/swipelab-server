using SwipeLab.Domain.Experiment.Enums;

namespace SwipeLab.Domain.Experiment;

public class Experiment
{
    public Guid ExperimentId { get; set; }
    public Guid ParticipantId { get; set; }
    public ExperimentState State { get; set; } = ExperimentState.SWIPING;
    public ExperimentType Type { get; set; } = ExperimentType.PREDEFINED_PROMPTS;
    public int SwipeCount { get; set; } = 0;
    public int ReflectionCount { get; set; } = 0;
    public Guid DatingProfileSetId { get; set; }
}