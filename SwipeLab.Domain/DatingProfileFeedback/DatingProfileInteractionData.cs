namespace SwipeLab.Domain.DatingProfileFeedback;

public class DatingProfileInteractionData(
    DatingProfile.DatingProfile profile,
    DatingProfileSwipe swipeInformation,
    DatingProfileReflection? userReflection)
{
    public DatingProfile.DatingProfile Profile { get; set; } = profile;
    public DatingProfileSwipe SwipeInformation { get; set; } = swipeInformation;
    public DatingProfileReflection? UserReflection { get; set; } = userReflection;
}