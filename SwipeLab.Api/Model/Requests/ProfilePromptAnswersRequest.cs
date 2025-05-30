namespace SwipeLab.Model.Requests;

public class ProfilePromptAnswersRequest
{
    public bool ChangedOpinion { get; set; }
    public int TimeSpentSeconds { get; set; }
    public int ProfileReviewCount { get; set; }
    public Dictionary<string, string> PromptAnswers { get; set; } = new();
}