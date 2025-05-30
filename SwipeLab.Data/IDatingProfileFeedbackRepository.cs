using SwipeLab.Domain.DatingProfileFeedback;

namespace SwipeLab.Data;

public interface IDatingProfileFeedbackRepository
{
    Task SaveDatingProfileSwipe(DatingProfileSwipe swipe);
    Task<DatingProfileSwipe?> GetDatingProfileSwipeByDatingProfileId(Guid datingProfileId);
    Task SaveDatingProfileReflection(DatingProfileReflection reflection);
    Task<DatingProfileReflection?> GetDatingProfileReflectionByDatingProfileId(Guid datingProfileId);
}