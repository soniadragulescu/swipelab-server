using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SwipeLab.Data.Postgres.Models.Entities;

namespace SwipeLab.Data.Postgres.Entities.DatingProfileFeedback
{
    public class DatingProfileReflection : BaseEntity
    {
        [Key] public Guid DatingProfileReflectionId { get; set; }

        public Guid? DatingProfileId { get; set; }
        public DatingProfile? DatingProfile { get; set; }

        public bool ChangedOpinion { get; set; }
        public int TimeSpentSeconds { get; set; }
        public int ProfileReviewCount { get; set; }
        [Column(TypeName = "jsonb")]
        public Dictionary<string, string> PromptAnswers { get; set; }

        public static Domain.DatingProfileFeedback.DatingProfileReflection ToDomain(DatingProfileReflection dpi)
        {
            return new Domain.DatingProfileFeedback.DatingProfileReflection()
            {
                DatingProfileReflectionId = dpi.DatingProfileReflectionId,
                DatingProfileId = dpi.DatingProfileId,
                ChangedOpinion = dpi.ChangedOpinion,
                PromptAnswers = dpi.PromptAnswers,
                TimeSpentSeconds = dpi.TimeSpentSeconds,
                ProfileReviewCount = dpi.ProfileReviewCount
            };
        }

        public static DatingProfileReflection FromDomain(Domain.DatingProfileFeedback.DatingProfileReflection dpi)
        {
            return new DatingProfileReflection()
            {
                DatingProfileReflectionId = dpi.DatingProfileReflectionId == Guid.Empty
                    ? Guid.NewGuid()
                    : dpi.DatingProfileReflectionId,
                DatingProfileId = dpi.DatingProfileId,
                ChangedOpinion = dpi.ChangedOpinion,
                PromptAnswers = dpi.PromptAnswers ?? new Dictionary<string, string>(),
                TimeSpentSeconds = dpi.TimeSpentSeconds,
                ProfileReviewCount = dpi.ProfileReviewCount
            };
        }
    }
}
