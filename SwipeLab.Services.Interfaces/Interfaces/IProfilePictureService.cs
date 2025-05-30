using SwipeLab.Domain.DatingProfile.Enums;
using SwipeLab.Domain.Enums;

namespace SwipeLab.Services.Interfaces.Interfaces;

public interface IProfilePictureService
{
    public Task<string> GetRandomProfilePictureUrl(Gender gender, Ethnicity ethnicity, int age);
}