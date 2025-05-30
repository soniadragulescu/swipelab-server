using System.ComponentModel.DataAnnotations;
using SwipeLab.Data.Postgres.Models.Entities;

namespace SwipeLab.Data.Postgres.Entities;

public class DatingProfileSet : BaseEntity
{
    [Key] public Guid DatingProfileSetId { get; set; }
    public ICollection<DatingProfile> DatingProfiles { get; set; } = new List<DatingProfile>();

    public static Domain.DatingProfile.DatingProfileSet ToDomain(DatingProfileSet entity)
    {
        var datingProfiles = entity.DatingProfiles
            .Select(DatingProfile.ToDomain)
            .OrderBy(dp => dp.DatingProfileId)
            .ToList();

        return new Domain.DatingProfile.DatingProfileSet
        {
            DatingProfileSetId = entity.DatingProfileSetId,
            DatingProfiles = datingProfiles
        };
    }

    public static DatingProfileSet FromDomain(Domain.DatingProfile.DatingProfileSet domain)
    {
        var entity = new DatingProfileSet
        {
            DatingProfileSetId = domain.DatingProfileSetId,
            DatingProfiles = domain.DatingProfiles.Select(DatingProfile.FromDomain).ToList()
        };

        foreach (var datingProfile in entity.DatingProfiles)
        {
            datingProfile.DatingProfileSetId = entity.DatingProfileSetId;
        }

        return entity;
    }
}