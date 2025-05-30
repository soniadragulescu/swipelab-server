using SwipeLab.Domain.DatingProfile.Constants;
using SwipeLab.Domain.DatingProfile.Enums;
using SwipeLab.Domain.Enums;

namespace SwipeLab.Domain.DatingProfile;

public class DatingProfileGenerationConstraints
{
    public AgeRange? AgeRange { get; set; }
    public Gender? Gender { get; set; }
    public HashSet<Ethnicity> AllowedEthnicities { get; set; } = [.. Enum.GetValues<Ethnicity>()];
    public HashSet<LookingFor> AllowedLookingFor { get; set; } = [.. Enum.GetValues<LookingFor>()];
    public HashSet<Education> AllowedEducation { get; set; } = [.. Enum.GetValues<Education>()];
    public HashSet<ActivityOftenRate> AllowedDrinking { get; set; } = [.. Enum.GetValues<ActivityOftenRate>()];
    public HashSet<ActivityOftenRate> AllowedSmoking { get; set; } = [.. Enum.GetValues<ActivityOftenRate>()];
    public HashSet<KidsPreference> AllowedKidsPreferences { get; set; } = [.. Enum.GetValues<KidsPreference>()];
    public HashSet<string> AllowedHobbies { get; set; } = HobbyConstants.AllHobbies;
    public HobbyCountRange HobbyCount { get; set; } = new(DatingProfileSetGenerationConstants.HOBBIES_COUNT_LOWER_LIMIT, DatingProfileSetGenerationConstants.HOBBIES_COUNT_UPPER_LIMIT);
}

public record AgeRange(int Min, int Max);

public record HeightRange(int MinCm, int MaxCm);

public record HobbyCountRange(int Min, int Max);