using SwipeLab.Domain.DatingProfileFeedback;

namespace SwipeLab.Model.Responses.Feedback;

public class SwipingFeedbackPrompts
{
    public List<string> Prompts { get; set; } = [];
    public SwipeState PreviousSwipeState { get; set; }
}