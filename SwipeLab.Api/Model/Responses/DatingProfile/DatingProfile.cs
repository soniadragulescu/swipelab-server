namespace SwipeLab.Model.Responses.DatingProfile;

public class DatingProfile : Domain.DatingProfile.DatingProfile
{
    public DatingProfile(Domain.DatingProfile.DatingProfile datingProfile)
    {
        DatingProfileId = datingProfile.DatingProfileId;
        DatingProfileSetId = datingProfile.DatingProfileSetId;
        Name = datingProfile.Name;
        Gender = datingProfile.Gender;
        PhotoUrl = datingProfile.PhotoUrl;
        Ethnicity = datingProfile.Ethnicity;
        Age = datingProfile.Age;
        LookingFor = datingProfile.LookingFor;
        Drinking = datingProfile.Drinking;
        Smoking = datingProfile.Smoking;
        Height = datingProfile.Height;
        KidsPreference = datingProfile.KidsPreference;
        Education = datingProfile.Education;
        Hobbies = datingProfile.Hobbies;
        PersonalityPrompts = datingProfile.PersonalityPrompts;
        Job = datingProfile.Job;
    }
}