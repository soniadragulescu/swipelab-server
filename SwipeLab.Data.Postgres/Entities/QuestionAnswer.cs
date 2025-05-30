using System.ComponentModel.DataAnnotations;
using SwipeLab.Data.Postgres.Models.Entities;

namespace SwipeLab.Data.Postgres.Entities
{
    public class QuestionAnswer : BaseEntity
    {
        [Key]
        public Guid QuestionAnswerId { get; set; }
        public Guid ExperimentId { get; set; }
        public Experiment Experiment { get; set; }
        public int QuestionNumber { get; set; }
        public required string Text { get; set; }
        public required string Answer { get; set; }

        public static Domain.Question.QuestionAnswer ToDomain(QuestionAnswer entity)
        {
            return new Domain.Question.QuestionAnswer
            {
                QuestionAnswerId = entity.QuestionAnswerId,
                ExperimentId = entity.ExperimentId,
                QuestionNumber = entity.QuestionNumber,
                Text = entity.Text,
                Answer = entity.Answer
            };
        }

        public static QuestionAnswer FromDomain(Domain.Question.QuestionAnswer domain)
        {
            return new QuestionAnswer
            {
                QuestionAnswerId = domain.QuestionAnswerId == Guid.Empty ? Guid.NewGuid() : domain.QuestionAnswerId,
                ExperimentId = domain.ExperimentId,
                QuestionNumber = domain.QuestionNumber,
                Text = domain.Text,
                Answer = domain.Answer
            };
        }
    }
}