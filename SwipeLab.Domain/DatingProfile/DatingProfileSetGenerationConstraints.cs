using SwipeLab.Domain.DatingProfile.Constants;
using SwipeLab.Domain.DatingProfile.Enums;
using SwipeLab.Domain.Enums;

namespace SwipeLab.Domain.DatingProfile;

public class DatingProfileSetGenerationConstraints
{
    public int SetSize { get; set; } = DatingProfileSetGenerationConstants.SET_SIZE;
    public Gender? FixedGender { get; set; }
    public Dictionary<Ethnicity, int>? EthnicityCounts { get; set; }
    public Dictionary<Gender, int>? GenderCounts { get; set; }
    public AgeRange AgeRange { get; set; } = new(18, 50);
    public HobbyCountRange HobbyCount { get; set; } = new(DatingProfileSetGenerationConstants.HOBBIES_COUNT_LOWER_LIMIT, DatingProfileSetGenerationConstants.HOBBIES_COUNT_UPPER_LIMIT);
    public HashSet<string> AllowedHobbies { get; set; } = HobbyConstants.AllHobbies;
    public bool ShouldRandomizeOrder { get; set; } = true;

    public void Validate()
    {
        if (SetSize <= 0)
            throw new ArgumentException("Set size must be positive");

        if (EthnicityCounts != null && EthnicityCounts.Values.Sum() != SetSize)
            throw new ArgumentException("Ethnicity counts must sum to set size");

        if (GenderCounts != null && GenderCounts.Values.Sum() != SetSize)
            throw new ArgumentException("Gender counts must sum to set size");

        if (FixedGender != null && GenderCounts != null)
            throw new ArgumentException("Cannot specify both FixedGender and GenderCounts");
    }
}