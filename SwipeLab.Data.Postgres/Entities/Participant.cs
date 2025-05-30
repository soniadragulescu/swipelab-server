using System.ComponentModel.DataAnnotations;
using SwipeLab.Domain.Enums;

namespace SwipeLab.Data.Postgres.Models.Entities
{
    public class Participant : BaseEntity
    {
        [Key] public Guid ParticipantId { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public Gender InterestedIn { get; set; }
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        public required string Ethnicity { get; set; }
        public required string CountryOfResidency { get; set; }
        public required string RecruitmentSource { get; set; }
        public UsageOfDatingApps UsageOfDatingApps { get; set; }
        public List<DatingApps>? KnownDatingApps { get; set; }
        public RelationshipStatus RelationshipStatus { get; set; }

        public static Domain.Participant.Participant ToDomain(Participant pt)
        {
            return new Domain.Participant.Participant
            {
                ParticipantId = pt.ParticipantId,
                DateOfBirth = pt.DateOfBirth,
                Gender = pt.Gender,
                InterestedIn = pt.InterestedIn,
                MinAge = pt.MinAge,
                MaxAge = pt.MaxAge,
                Ethnicity = pt.Ethnicity,
                CountryOfResidency = pt.CountryOfResidency,
                RecruitmentSource = pt.RecruitmentSource,
                UsageOfDatingApps = pt.UsageOfDatingApps,
                KnownDatingApps = pt.KnownDatingApps?
                    .ToList(),
                RelationshipStatus = pt.RelationshipStatus
            };
        }

        public static Participant FromDomain(Domain.Participant.Participant domain)
        {
            return new Participant
            {
                ParticipantId = domain.ParticipantId == Guid.Empty ? Guid.NewGuid() : domain.ParticipantId,
                DateOfBirth = domain.DateOfBirth,
                Gender = domain.Gender,
                InterestedIn = domain.InterestedIn,
                MinAge = domain.MinAge,
                MaxAge = domain.MaxAge,
                Ethnicity = domain.Ethnicity,
                CountryOfResidency = domain.CountryOfResidency,
                RecruitmentSource = domain.RecruitmentSource,
                UsageOfDatingApps = domain.UsageOfDatingApps,
                KnownDatingApps = domain.KnownDatingApps?
                    .ToList(),
                RelationshipStatus = domain.RelationshipStatus
            };
        }
    }
}