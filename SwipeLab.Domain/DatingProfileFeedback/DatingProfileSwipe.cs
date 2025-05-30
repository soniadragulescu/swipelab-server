namespace SwipeLab.Domain.DatingProfileFeedback
{
    public class DatingProfileSwipe
    {
        public Guid DatingProfileSwipeId { get; set; }
        public Guid? DatingProfileId { get; set; }
        public SwipeState SwipeState { get; set; }
        public int TimeSpentSeconds { get; set; }
    }
}