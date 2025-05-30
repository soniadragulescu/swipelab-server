using SwipeLab.Domain.DatingProfileFeedback;

namespace SwipeLab.Services.Interfaces.Interfaces
{
    public interface IDatingProfileFeedbackService
    {
        Task RegisterSwipeAsync(DatingProfileSwipe datingProfileSwipe);
        Task RegisterReflectionAsync(DatingProfileReflection datingProfileReflection);
    }
}