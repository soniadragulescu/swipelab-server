using SwipeLab.Domain.DatingProfile;

namespace SwipeLab.Data
{
    public interface IDatingProfileRepository
    {
        Task<DatingProfile?> GetDatingProfile(Guid datingProfileId);
    }
}