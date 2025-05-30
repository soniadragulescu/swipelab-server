using System.ComponentModel.DataAnnotations;
using SwipeLab.Data.Postgres.Models.Entities;
using SwipeLab.Domain.DatingProfileFeedback;

namespace SwipeLab.Data.Postgres.Entities.DatingProfileFeedback
{
    public class DatingProfileSwipe : BaseEntity
    {
        [Key] public Guid DatingProfileSwipeId { get; set; }

        public Guid? DatingProfileId { get; set; }
        public DatingProfile? DatingProfile { get; set; }

        public SwipeState SwipeState { get; set; }
        public int TimeSpentSeconds { get; set; }

        public static Domain.DatingProfileFeedback.DatingProfileSwipe ToDomain(DatingProfileSwipe dpi)
        {
            return new Domain.DatingProfileFeedback.DatingProfileSwipe
            {
                DatingProfileSwipeId = dpi.DatingProfileSwipeId,
                DatingProfileId = dpi.DatingProfileId,
                SwipeState = dpi.SwipeState,
                TimeSpentSeconds = dpi.TimeSpentSeconds
            };
        }

        public static DatingProfileSwipe FromDomain(Domain.DatingProfileFeedback.DatingProfileSwipe dpi)
        {
            return new DatingProfileSwipe
            {
                DatingProfileSwipeId = dpi.DatingProfileSwipeId == Guid.Empty ? Guid.NewGuid() : dpi.DatingProfileSwipeId,
                DatingProfileId = dpi.DatingProfileId,
                SwipeState = dpi.SwipeState,
                TimeSpentSeconds = dpi.TimeSpentSeconds
            };
        }
    }
}
