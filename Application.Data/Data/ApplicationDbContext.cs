using Application.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Data.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Metric> Metrics { get; set; }
        public DbSet<Result> Results { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Metric>()
                .ToTable("Values")
                .HasIndex(m => m.FileName);

            modelBuilder.Entity<Result>()
                .ToTable("Results")
                .HasIndex(r => r.FileName);
        }
    }
}