using Microsoft.EntityFrameworkCore;
using CommitCast.Core.Models;

namespace CommitCast.Core.Data;

public class CommitCastDbContext : DbContext
{
    public CommitCastDbContext(DbContextOptions<CommitCastDbContext> options) 
        : base(options)
    {
    }

    public DbSet<Post> Posts => Set<Post>();
    public DbSet<MonitoredRepository> MonitoredRepositories => Set<MonitoredRepository>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RepoUrl).IsRequired().HasMaxLength(500);
            entity.Property(e => e.CommitHash).IsRequired().HasMaxLength(40);
            entity.Property(e => e.CommitMessage).IsRequired().HasMaxLength(2000);
            entity.Property(e => e.CommitAuthor).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Status).HasConversion<string>();
            entity.HasIndex(e => e.CommitHash);
            entity.HasIndex(e => e.Status);
        });

        modelBuilder.Entity<MonitoredRepository>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RepoUrl).IsRequired().HasMaxLength(500);
            entity.HasIndex(e => e.RepoUrl).IsUnique();
        });
    }
}
