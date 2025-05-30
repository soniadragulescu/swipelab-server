using System.ComponentModel.DataAnnotations;
using SwipeLab.Domain.Enums;

namespace SwipeLab.Data.Postgres.Models.Entities
{
    public class BaseEntity
    {
        public RowStatus RowStatus { get; set; }
        public DateTime RowCreatedUtc { get; set; }
        [ConcurrencyCheck]
        public DateTime RowLastUpdated { get; set; }
    }
}
