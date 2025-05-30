using SwipeLab.Domain.DatingProfile;

namespace SwipeLab.Services.Interfaces.Interfaces;

public interface IDatingProfileGenerationService
{
    public Task<DatingProfileSet> GenerateDatingProfileSetAsync(
        DatingProfileSetGenerationConstraints datingProfileSetGenerationConstraints);
    public Task<DatingProfile> GenerateDatingProfileAsync(
        DatingProfileGenerationConstraints datingProfileGenerationConstraints);
}