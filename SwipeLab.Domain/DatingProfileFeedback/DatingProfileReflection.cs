namespace SwipeLab.Domain.DatingProfileFeedback;

public class DatingProfileReflection
{
    public Guid DatingProfileReflectionId { get; set; }
    public Guid? DatingProfileId { get; set; }
    public bool ChangedOpinion { get; set; }
    public int TimeSpentSeconds { get; set; }
    public int ProfileReviewCount { get; set; }
    public Dictionary<string, string>? PromptAnswers { get; set; }
}