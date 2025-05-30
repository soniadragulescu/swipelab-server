using SwipeLab.Domain.DatingProfileFeedback;

namespace SwipeLab.Model.Requests;

public class ProfileSwipeRequest
{
    public SwipeState SwipeState { get; set; }
    public int TimeSpentSeconds { get; set; }
}