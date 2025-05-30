namespace SwipeLab.Domain.Question
{
    public class QuestionAnswer
    {
        public Guid QuestionAnswerId { get; set; }
        public Guid ExperimentId { get; set; }
        public int QuestionNumber { get; set; }
        public string? Text { get; set; }
        public required string Answer { get; set; }
    }
}
