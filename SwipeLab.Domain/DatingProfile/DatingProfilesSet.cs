namespace SwipeLab.Domain.DatingProfile;

public class DatingProfileSet
{
    public Guid DatingProfileSetId { get; set; } = Guid.Empty;
    public List<DatingProfile> DatingProfiles { get; set; } = [];
}