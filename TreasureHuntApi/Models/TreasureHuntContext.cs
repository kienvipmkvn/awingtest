using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;

namespace TreasureHuntApi.Models
{
    public class TreasureHuntContext : DbContext
    {
        public TreasureHuntContext(DbContextOptions<TreasureHuntContext> options) : base(options) { }

        public DbSet<TreasureHuntInput> Inputs { get; set; }
        public DbSet<TreasureHuntResult> Results { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var options = new JsonSerializerOptions();

            var converter = new ValueConverter<int[][], string>(
                v => JsonSerializer.Serialize(v, options),
                v => JsonSerializer.Deserialize<int[][]>(v, options)!
            );

            var comparer = new ValueComparer<int[][]>(
                (a, b) => JsonSerializer.Serialize(a, options) == JsonSerializer.Serialize(b, options),
                v => v == null ? 0 : JsonSerializer.Serialize(v, options).GetHashCode(),
                v => JsonSerializer.Deserialize<int[][]>(JsonSerializer.Serialize(v, options), options)!
            );

            modelBuilder.Entity<TreasureHuntInput>()
                .Property(e => e.Matrix)
                .HasConversion(converter)
                .Metadata.SetValueComparer(comparer);
        }
    }
} 