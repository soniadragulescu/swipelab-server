using System.Security.Cryptography;
using SwipeLab.Domain.DatingProfile;
using SwipeLab.Domain.DatingProfile.Constants;
using SwipeLab.Domain.DatingProfile.Enums;
using SwipeLab.Domain.DatingProfile.Utils;
using SwipeLab.Domain.Enums;
using SwipeLab.Services.Interfaces.Interfaces;
using SwipeLab.Services.Utils;

namespace SwipeLab.Services.Services;

public class DatingProfileGenerationService : IDatingProfileGenerationService
{
    private readonly IProfilePictureService _profilePictureService;

    public DatingProfileGenerationService(IProfilePictureService profilePictureService)
    {
        _profilePictureService = profilePictureService;
    }

    public async Task<DatingProfileSet> GenerateDatingProfileSetAsync(
        DatingProfileSetGenerationConstraints datingProfileSetGenerationConstraints)
    {
        datingProfileSetGenerationConstraints.Validate();
        var profiles = new List<DatingProfile>();

        // Generate ethnicity distribution
        var ethnicityQueue = DistributionQueueUtils.CreateDistributionQueue(
            datingProfileSetGenerationConstraints.EthnicityCounts,
            GetRandomEnum<Ethnicity>,
            datingProfileSetGenerationConstraints.SetSize);

        // Generate gender distribution
        var genderQueue = DistributionQueueUtils.CreateDistributionQueue(
            datingProfileSetGenerationConstraints.GenderCounts,
            () => datingProfileSetGenerationConstraints.FixedGender ?? GetRandomFixedGender(),
            datingProfileSetGenerationConstraints.SetSize);

        for (int i = 0; i < datingProfileSetGenerationConstraints.SetSize; i++)
        {
            var profileConstraints = new DatingProfileGenerationConstraints
            {
                AgeRange = datingProfileSetGenerationConstraints.AgeRange,
                HobbyCount = datingProfileSetGenerationConstraints.HobbyCount,
                AllowedHobbies = datingProfileSetGenerationConstraints.AllowedHobbies,
                Gender = genderQueue.Dequeue(),
                AllowedEthnicities = [ethnicityQueue.Dequeue()]
            };

            var profile = await GenerateDatingProfileAsync(profileConstraints);

            profiles.Add(profile);
        }

        EnhanceProfilesWithPrompts(profiles);

        return new DatingProfileSet { DatingProfiles = profiles };
    }

    public async Task<DatingProfile> GenerateDatingProfileAsync(
        DatingProfileGenerationConstraints datingProfileGenerationConstraints)
    {
        var gender = datingProfileGenerationConstraints.Gender ?? GetRandomFixedGender();
        var ethnicity = GetRandomEthnicity(datingProfileGenerationConstraints.AllowedEthnicities);
        var age = GetRandomAge(datingProfileGenerationConstraints.AgeRange);
        var education = GetRandomEducation(age, datingProfileGenerationConstraints.AllowedEducation);

        var profile = new DatingProfile
        {
            DatingProfileId = Guid.NewGuid(),
            Name = NameGenerator.GetRandomName(gender, ethnicity),
            Ethnicity = ethnicity,
            Age = age,
            Height = GetRandomHeight(gender),
            Gender = gender,
            LookingFor = GetRandomLookingFor(datingProfileGenerationConstraints.AllowedLookingFor),
            Drinking = GetRandomActivityRate(datingProfileGenerationConstraints.AllowedDrinking),
            Smoking = GetRandomActivityRate(datingProfileGenerationConstraints.AllowedSmoking),
            KidsPreference = GetRandomKidsPreference(datingProfileGenerationConstraints.AllowedKidsPreferences),
            Education = education,
            Hobbies = GetRandomHobbies(datingProfileGenerationConstraints.AllowedHobbies,
                datingProfileGenerationConstraints.HobbyCount),
            Job = GetRandomJob(education),
            PhotoUrl = await _profilePictureService.GetRandomProfilePictureUrl(gender, ethnicity, age)
        };

        return profile;
    }

    private string GetRandomJob(Education education)
    {
        var random = new Random();
        var allowedJobs = JobConstants.GetAllowedJob(education);

        return allowedJobs.ElementAt(random.Next(allowedJobs.Count));
    }

    private void EnhanceProfilesWithPrompts(List<DatingProfile> profiles)
    {
        var random = new Random();
        var promptsList = PromptsConstants.Prompts.ToList();
        promptsList = promptsList.OrderBy(x => random.Next()).ToList();

        int currentIndex = 0;
        int promptsCount = promptsList.Count;

        foreach (var profile in profiles)
        {
            var selectedPrompts = new Dictionary<string, string>();

            for (int i = 0; i < DatingProfileSetGenerationConstants.PROMPTS_PER_PROFILE; i++)
            {
                var prompt = promptsList[(currentIndex + i) % promptsCount];
                selectedPrompts.Add(prompt.Beginning, prompt.Continuation);
            }

            profile.PersonalityPrompts = selectedPrompts;
            currentIndex += DatingProfileSetGenerationConstants.PROMPTS_PER_PROFILE;
        }
    }

    public Ethnicity GetRandomEthnicity(HashSet<Ethnicity>? allowed)
    {
        return allowed?.Count > 0
            ? allowed.ElementAt(RandomNumberGenerator.GetInt32(allowed.Count))
            : GetRandomEnum<Ethnicity>();
    }

    public int GetRandomAge(AgeRange? range)
    {
        var min = range?.Min ?? 18;
        var max = range?.Max ?? 45;
        return RandomNumberGenerator.GetInt32(min, max + 1);
    }

    public int GetRandomHeight(Gender gender)
    {
        double mean = gender == Gender.Male
            ? DatingProfileSetGenerationConstants.MALE_HEIGHT_MEAN
            : DatingProfileSetGenerationConstants.FEMALE_HEIGHT_MEAN;

        return (int)Math.Round(NextGaussian(mean, DatingProfileSetGenerationConstants.HEIGHT_STD_DEV));
    }


    private static double NextGaussian(double mean, double stdDev)
    {
        var random = new Random();

        // Box-Muller transform
        double u1 = 1.0 - random.NextDouble(); // uniform(0,1] random doubles
        double u2 = 1.0 - random.NextDouble();
        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                               Math.Sin(2.0 * Math.PI * u2); // random normal(0,1)

        return mean + stdDev * randStdNormal;
    }

    public List<string> GetRandomHobbies(HashSet<string> availableHobbies, HobbyCountRange countRange)
    {
        // Validate count range
        if (countRange.Min < 1) throw new ArgumentException("Minimum hobbies cannot be less than 1");
        if (countRange.Max > availableHobbies.Count)
        {
            countRange = countRange with { Max = availableHobbies.Count };
        }

        if (countRange.Min > countRange.Max)
        {
            countRange = countRange with { Min = countRange.Max };
        }

        // Determine random count within range
        var hobbyCount = RandomNumberGenerator.GetInt32(
            countRange.Min,
            countRange.Max + 1); // Max is inclusive

        // Select random hobbies
        return availableHobbies
            .OrderBy(_ => RandomNumberGenerator.GetInt32(int.MaxValue))
            .Take(hobbyCount)
            .ToList();
    }

    public T GetRandomEnum<T>() where T : struct, Enum
    {
        var values = Enum.GetValues<T>();
        return values[RandomNumberGenerator.GetInt32(values.Length)];
    }

    public T GetRandomFromSet<T>(HashSet<T>? allowed, Func<T> defaultGetter)
    {
        return allowed?.Count > 0
            ? allowed.ElementAt(RandomNumberGenerator.GetInt32(allowed.Count))
            : defaultGetter();
    }

    // Helper methods for specific enums
    public LookingFor GetRandomLookingFor(HashSet<LookingFor>? allowed)
        => GetRandomFromSet(allowed, GetRandomEnum<LookingFor>);

    public ActivityOftenRate GetRandomActivityRate(HashSet<ActivityOftenRate>? allowed)
        => GetRandomFromSet(allowed, GetRandomEnum<ActivityOftenRate>);

    public KidsPreference GetRandomKidsPreference(HashSet<KidsPreference>? allowed)
        => GetRandomFromSet(allowed, GetRandomEnum<KidsPreference>);

    public Education GetRandomEducation(int age, HashSet<Education> allowed)
    {
        var allowedEduction = age > 22
            ? allowed
            : new HashSet<Education>
            {
                Education.HIGH_SCHOOL,
                Education.BACHELOR
            };

        return GetRandomFromSet(allowedEduction, GetRandomEnum<Education>);
    }

    private Gender GetRandomFixedGender()
    {
        return RandomNumberGenerator.GetInt32(3) % 2 == 1 ? Gender.Female : Gender.Male;
    }
}