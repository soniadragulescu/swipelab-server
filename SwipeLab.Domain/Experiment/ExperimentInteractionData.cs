using SwipeLab.Domain.DatingProfileFeedback;

namespace SwipeLab.Domain.Experiment;

public class ExperimentInteractionData
{
    public int TotalProfileCount { get; set; } = 0;
    public List<DatingProfileInteractionData> DatingProfileInteractions { get; set; } = [];
}