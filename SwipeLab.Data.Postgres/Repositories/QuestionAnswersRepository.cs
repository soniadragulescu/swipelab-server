using Microsoft.Extensions.Logging;
using SwipeLab.Data.Postgres.Configuration;
using SwipeLab.Domain.Question;

namespace SwipeLab.Data.Postgres.Repositories
{
    public class QuestionAnswersRepository : IQuestionAnswersRepository
    {
        private readonly ILogger<QuestionAnswersRepository> _logger;
        private readonly ApplicationDbContext _context;

        public QuestionAnswersRepository(ILogger<QuestionAnswersRepository> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task SaveQuestionAnswers(List<QuestionAnswer> answers)
        {
            _logger.LogInformation("Saving answers...");

            var entities = answers.Select(a => Entities.QuestionAnswer.FromDomain(a)).ToList();

            await _context.QuestionAnswers.AddRangeAsync(entities);

            await _context.SaveChangesAsync();

            _logger.LogInformation("Answers saved successfully.");
        }
    }
}
