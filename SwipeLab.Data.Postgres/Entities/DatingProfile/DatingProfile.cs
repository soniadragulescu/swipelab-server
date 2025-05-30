using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SwipeLab.Data.Postgres.Models.Entities;
using SwipeLab.Domain.DatingProfile.Enums;
using SwipeLab.Domain.Enums;

namespace SwipeLab.Data.Postgres.Entities
{
    public class DatingProfile : BaseEntity
    {
        [Key] public Guid DatingProfileId { get; set; }

        public Guid? DatingProfileSetId { get; set; }
        public DatingProfileSet? DatingProfileSet { get; set; }
        public required string Name { get; set; }
        public required string PhotoUrl { get; set; }
        public Ethnicity Ethnicity { get; set; }
        public int Age { get; set; }
        public int Height { get; set; }
        public LookingFor LookingFor { get; set; }
        public ActivityOftenRate Drinking { get; set; }
        public ActivityOftenRate Smoking { get; set; }
        public KidsPreference KidsPreference { get; set; }
        public Education Education { get; set; }
        public required List<string> Hobbies { get; set; }
        public Gender Gender { get; set; }
        public string? Job { get; set; }

        [Column(TypeName = "jsonb")]
        public Dictionary<string, string>? PersonalityPrompts { get; set; }

        public static Domain.DatingProfile.DatingProfile ToDomain(DatingProfile entity)
        {
            return new Domain.DatingProfile.DatingProfile
            {
                DatingProfileId = entity.DatingProfileId,
                DatingProfileSetId = entity.DatingProfileSetId,
                Name = entity.Name,
                PhotoUrl = entity.PhotoUrl,
                Age = entity.Age,
                Ethnicity = entity.Ethnicity,
                LookingFor = entity.LookingFor,
                Drinking = entity.Drinking,
                Smoking = entity.Smoking,
                Height = entity.Height,
                KidsPreference = entity.KidsPreference,
                Education = entity.Education,
                Hobbies = new List<string>(entity.Hobbies),
                Gender = entity.Gender,
                Job = entity.Job,
                PersonalityPrompts = entity.PersonalityPrompts ?? new Dictionary<string, string>()
            };
        }

        public static DatingProfile FromDomain(Domain.DatingProfile.DatingProfile domain)
        {
            return new DatingProfile
            {
                DatingProfileId = domain.DatingProfileId == Guid.Empty ? Guid.NewGuid() : domain.DatingProfileId,
                DatingProfileSetId = domain.DatingProfileSetId,
                Name = domain.Name,
                PhotoUrl = domain.PhotoUrl,
                Age = domain.Age,
                Ethnicity = domain.Ethnicity,
                LookingFor = domain.LookingFor,
                Drinking = domain.Drinking,
                Smoking = domain.Smoking,
                Height = domain.Height,
                KidsPreference = domain.KidsPreference,
                Education = domain.Education,
                Hobbies = [.. domain.Hobbies],
                Gender = domain.Gender,
                Job = domain.Job,
                PersonalityPrompts = domain.PersonalityPrompts ?? new Dictionary<string, string>()
            };
        }
    }
}