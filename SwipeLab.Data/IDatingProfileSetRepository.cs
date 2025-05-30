using SwipeLab.Domain.DatingProfile;

namespace SwipeLab.Data;

public interface IDatingProfileSetRepository
{
    Task<Guid> SaveDatingProfileSet(DatingProfileSet datingProfileSet);
    Task<DatingProfileSet?> GetDatingProfileSetById(Guid datingProfileSetId);
}