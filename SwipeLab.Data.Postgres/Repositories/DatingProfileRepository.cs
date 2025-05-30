using Microsoft.Extensions.Logging;
using SwipeLab.Data.Postgres.Configuration;
using DatingProfile = SwipeLab.Domain.DatingProfile.DatingProfile;

namespace SwipeLab.Data.Postgres.Repositories
{
    public class DatingProfileRepository : IDatingProfileRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DatingProfileRepository> _logger;

        public DatingProfileRepository(ApplicationDbContext context, ILogger<DatingProfileRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<DatingProfile?> GetDatingProfile(Guid profileId)
        {
            try
            {
                _logger.LogInformation("Getting profile with ID: {ProfileId}", profileId);

                var entity = await _context.DatingProfiles.FindAsync(profileId);
                if (entity == null)
                {
                    _logger.LogWarning("Profile with ID: {ProfileId} not found", profileId);
                    return null;
                }

                var response = Entities.DatingProfile.ToDomain(entity);

                _logger.LogInformation("Profile with ID: {ProfileId} found. Response: {@Response}", profileId, response);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving profile with ID: {ProfileId}", profileId);
                throw;
            }
        }
    }
}