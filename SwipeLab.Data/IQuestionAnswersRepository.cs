using SwipeLab.Domain.Question;

namespace SwipeLab.Data
{
    public interface IQuestionAnswersRepository
    {
        Task SaveQuestionAnswers(List<QuestionAnswer> answers);
    }
}
