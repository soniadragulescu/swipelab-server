using SwipeLab.Domain.DatingProfile.Enums;
using SwipeLab.Domain.Enums;

namespace SwipeLab.Domain.DatingProfile
{
    public class DatingProfile
    {
        public Guid DatingProfileId { get; set; }
        public Guid? DatingProfileSetId { get; set; }
        public string? Name { get; set; }
        public string? PhotoUrl { get; set; }
        public Ethnicity Ethnicity { get; set; }
        public int Age { get; set; }
        public int Height { get; set; }
        public Gender Gender { get; set; }
        public LookingFor LookingFor { get; set; }
        public ActivityOftenRate Drinking { get; set; }
        public ActivityOftenRate Smoking { get; set; }
        public KidsPreference KidsPreference { get; set; }
        public Education Education { get; set; }
        public List<string> Hobbies { get; set; } = [];
        public Dictionary<string, string>? PersonalityPrompts { get; set; }
        public string? Job { get; set; }
    }
}