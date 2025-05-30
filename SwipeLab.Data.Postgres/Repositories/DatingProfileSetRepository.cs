using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SwipeLab.Data.Postgres.Configuration;
using SwipeLab.Domain.DatingProfile;

namespace SwipeLab.Data.Postgres.Repositories;

public class DatingProfileSetRepository : IDatingProfileSetRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DatingProfileSetRepository> _logger;

    public DatingProfileSetRepository(ILogger<DatingProfileSetRepository> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Guid> SaveDatingProfileSet(DatingProfileSet datingProfileSet)
    {
        try
        {
            _logger.LogInformation("Saving dating profile with data: {@DatingProfileSet}", datingProfileSet);

            var entity = Entities.DatingProfileSet.FromDomain(datingProfileSet);

            _context.DatingProfileSets.Add(entity);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Profile created successfully with ID: {DatingProfileSetId}", entity.DatingProfileSetId);
            return entity.DatingProfileSetId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating profile: {@DatingProfileSet}", datingProfileSet);
            throw;
        }
    }

    public async Task<DatingProfileSet?> GetDatingProfileSetById(Guid datingProfileSetId)
    {
        var datingProfileSet = await _context.DatingProfileSets
            .Include(dps => dps.DatingProfiles)
            .FirstOrDefaultAsync(dps => dps.DatingProfileSetId == datingProfileSetId);

        if (datingProfileSet != null)
        {
            return Entities.DatingProfileSet.ToDomain(datingProfileSet);
        }

        return null;
    }
}