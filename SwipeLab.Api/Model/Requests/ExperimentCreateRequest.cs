using SwipeLab.Domain.Enums;

namespace SwipeLab.Model.Requests;

public class ExperimentCreateRequest
{
    public DateOnly DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public Gender InterestedIn { get; set; }
    public int MinAge { get; set; }
    public int MaxAge { get; set; }
    public required string Ethnicity { get; set; }
    public required string CountryOfResidency { get; set; }
    public required string RecruitmentSource { get; set; }
    public required string OnboardingConfidence { get; set; }
    public required string OnboardingComfortable { get; set; }
    public UsageOfDatingApps UsageOfDatingApps { get; set; }
    public List<DatingApps>? KnownDatingApps { get; set; }
    public RelationshipStatus RelationshipStatus { get; set; }
}