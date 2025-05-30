using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SwipeLab.Data.Postgres.Configuration;
using SwipeLab.Domain.DatingProfileFeedback;

namespace SwipeLab.Data.Postgres.Repositories;

public class DatingProfileFeedbackRepository : IDatingProfileFeedbackRepository
{
    private readonly ILogger<DatingProfileFeedbackRepository> _logger;
    private readonly ApplicationDbContext _context;

    public DatingProfileFeedbackRepository(ILogger<DatingProfileFeedbackRepository> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task SaveDatingProfileSwipe(DatingProfileSwipe swipe)
    {
        _logger.LogInformation("Creating DatingProfileSwipe with data: {@DatingProfileSwipe}", swipe);

        var entity = Entities.DatingProfileFeedback.DatingProfileSwipe.FromDomain(swipe);

        _context.DatingProfileSwipes.Add(entity);

        await _context.SaveChangesAsync();

        _logger.LogInformation("DatingProfileSwipe created successfully with Id: {DatingProfileSwipeId}",
            entity.DatingProfileSwipeId);

        swipe.DatingProfileSwipeId = entity.DatingProfileSwipeId;
    }

    public async Task<DatingProfileSwipe?> GetDatingProfileSwipeByDatingProfileId(Guid datingProfileId)
    {
        _logger.LogInformation("Getting DatingProfileSwipe with DatingProfileId: {DatingProfileId}", datingProfileId);

        var entity = await _context.DatingProfileSwipes
            .FirstOrDefaultAsync(d => d.DatingProfileId == datingProfileId);

        if (entity == null)
        {
            return null;
        }

        return Entities.DatingProfileFeedback.DatingProfileSwipe.ToDomain(entity);
    }

    public async Task SaveDatingProfileReflection(DatingProfileReflection reflection)
    {
        _logger.LogInformation("Creating DatingProfileReflection with data: {@DatingProfileReflection}", reflection);

        var entity = Entities.DatingProfileFeedback.DatingProfileReflection.FromDomain(reflection);

        _context.DatingProfileReflections.Add(entity);

        await _context.SaveChangesAsync();

        _logger.LogInformation("DatingProfileReflection created successfully with Id: {DatingProfileReflectionId}",
            entity.DatingProfileReflectionId);

        reflection.DatingProfileReflectionId = entity.DatingProfileReflectionId;
    }

    public async Task<DatingProfileReflection?> GetDatingProfileReflectionByDatingProfileId(Guid datingProfileId)
    {
        _logger.LogInformation("Getting DatingProfileReflection with DatingProfileId: {DatingProfileId}", datingProfileId);

        var entity = await _context.DatingProfileReflections
            .FirstOrDefaultAsync(d => d.DatingProfileId == datingProfileId);

        if (entity == null)
        {
            return null;
        }

        return Entities.DatingProfileFeedback.DatingProfileReflection.ToDomain(entity);
    }
}