using SwipeLab.Domain.Enums;

namespace SwipeLab.Domain.Participant
{
    public class Participant
    {
        public Guid ParticipantId { get; set; } = Guid.Empty;
        public DateOnly DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public Gender InterestedIn { get; set; }
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        public int Height { get; set; }
        public required string Ethnicity { get; set; }
        public required string CountryOfResidency { get; set; }
        public required string RecruitmentSource { get; set; }
        public UsageOfDatingApps UsageOfDatingApps { get; set; }
        public List<DatingApps>? KnownDatingApps { get; set; }
        public RelationshipStatus RelationshipStatus { get; set; }
    }
}