using System.Reflection;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using SwipeLab.Data.Postgres.Entities;
using SwipeLab.Data.Postgres.Entities.DatingProfileFeedback;
using SwipeLab.Data.Postgres.Models.Entities;
using SwipeLab.Domain.Enums;

namespace SwipeLab.Data.Postgres.Configuration
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Experiment> Experiments { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<DatingProfileSet> DatingProfileSets { get; set; }
        public DbSet<DatingProfile> DatingProfiles { get; set; }
        public DbSet<DatingProfileSwipe> DatingProfileSwipes { get; set; }
        public DbSet<DatingProfileReflection> DatingProfileReflections { get; set; }
        public DbSet<QuestionAnswer> QuestionAnswers { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            HandleBaseEntities();

            return base.SaveChangesAsync(cancellationToken);
        }

        private void HandleBaseEntities()
        {
            var currentDateTime = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

            var createdProperties = new Dictionary<string, object>
        {
            { "RowStatus", RowStatus.Active },
            { "RowCreatedUtc", currentDateTime }
        };

            var deletedProperties = new Dictionary<string, object>
        {
            { "RowStatus", RowStatus.Deleted },
            { "RowDeletedUtc", currentDateTime }
        };

            var trackedEntities =
                    from entry in base.ChangeTracker.Entries()
                    where entry.Entity is BaseEntity
                    select entry
                ;

            var filteredEntities = trackedEntities
                .Where(entry
                    => entry.State is EntityState.Added or EntityState.Deleted
                );

            foreach (var entry in filteredEntities)
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues.SetValues(createdProperties);
                        break;
                    case EntityState.Deleted:
                        entry.CurrentValues.SetValues(deletedProperties);
                        entry.State = EntityState.Modified;
                        break;
                }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Add query filters
            modelBuilder.Entity<Experiment>().HasQueryFilter(a => a.RowStatus != RowStatus.Deleted);
            modelBuilder.Entity<Participant>().HasQueryFilter(a => a.RowStatus != RowStatus.Deleted);
            modelBuilder.Entity<DatingProfileSet>().HasQueryFilter(a => a.RowStatus != RowStatus.Deleted);
            modelBuilder.Entity<DatingProfile>().HasQueryFilter(a => a.RowStatus != RowStatus.Deleted);
            modelBuilder.Entity<DatingProfileSwipe>().HasQueryFilter(a => a.RowStatus != RowStatus.Deleted);
            modelBuilder.Entity<DatingProfileReflection>().HasQueryFilter(a => a.RowStatus != RowStatus.Deleted);
            modelBuilder.Entity<QuestionAnswer>().HasQueryFilter(a => a.RowStatus != RowStatus.Deleted);

            modelBuilder.Entity<DatingProfile>()
                .Property(e => e.PersonalityPrompts)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, (JsonSerializerOptions)null)
                );

            modelBuilder.Entity<DatingProfile>()
                .Property(e => e.PersonalityPrompts)
                .Metadata
                .SetValueComparer(new ValueComparer<Dictionary<string, string>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToDictionary())); // Create a snapshot of the dict

            modelBuilder.Entity<DatingProfileReflection>()
                .Property(e => e.PromptAnswers)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, (JsonSerializerOptions)null)
                );

            modelBuilder.Entity<DatingProfileReflection>()
                .Property(e => e.PromptAnswers)
                .Metadata
                .SetValueComparer(new ValueComparer<Dictionary<string, string>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToDictionary())); // Create a snapshot of the dict

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
             => optionsBuilder
                    .ConfigureWarnings(c => c.Log((RelationalEventId.CommandExecuted, LogLevel.Debug)));
    }
}