using AutoMapper;
using Microsoft.Extensions.Logging;
using SwipeLab.Data.Postgres.Configuration;
using SwipeLab.Domain.Participant;

namespace SwipeLab.Data.Postgres.Repositories
{
    public class ParticipantRespository : IParticipantRepository
    {
        private readonly ILogger<ParticipantRespository> _logger;
        private readonly ApplicationDbContext _context;

        public ParticipantRespository(ApplicationDbContext context,
            ILogger<ParticipantRespository> logger,
            IMapper mapper)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Guid> CreateParticipant(Participant participant)
        {
            _logger.LogInformation("Starting to create participant with data: {participant}", participant);

            var entity = Models.Entities.Participant.FromDomain(participant);

            _context.Participants.Add(entity);

            await _context.SaveChangesAsync();

            _logger.LogInformation("Participant created succesfully with ID: {entity.ParticipantId}",
                entity.ParticipantId);

            return entity.ParticipantId;
        }

        public async Task<Participant?> GetParticipant(Guid participantId)
        {
            _logger.LogInformation("Getting participant with ID: {participantId}", participantId);

            var entity = await _context.Participants.FindAsync(participantId);

            if (entity == null)
            {
                _logger.LogInformation("Participant with ID: {participantId} not found", participantId);
                return null;
            }

            var response = Models.Entities.Participant.ToDomain(entity);

            _logger.LogInformation("Participant with ID: {participantId} found. Response: {response}",
                response.ParticipantId, response);

            return response;
        }
    }
}