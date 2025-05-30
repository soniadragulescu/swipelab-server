namespace SwipeLab.Model.Requests
{
    public class QuestionAnswerItemRequest
    {
        public int QuestionNumber { get; set; }
        public required string Text { get; set; }
        public required string Answer { get; set; }
    }
}
