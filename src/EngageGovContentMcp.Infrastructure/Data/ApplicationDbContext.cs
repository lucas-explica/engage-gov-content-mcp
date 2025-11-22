namespace EngageGovContentMcp.Infrastructure.Data;

using EngageGovContentMcp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Database context for the application.
/// Configures entity mappings and database behavior.
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<ContentItem> ContentItems => Set<ContentItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure ContentItem entity
        modelBuilder.Entity<ContentItem>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.Description)
                .HasMaxLength(500);

            entity.Property(e => e.Content)
                .IsRequired();

            entity.Property(e => e.Category)
                .HasMaxLength(100);

            entity.Property(e => e.IsPublished)
                .IsRequired();

            entity.Property(e => e.CreatedAt)
                .IsRequired();

            entity.Property(e => e.UpdatedAt)
                .IsRequired();

            // Index for faster queries
            entity.HasIndex(e => e.Category);
            entity.HasIndex(e => e.IsPublished);
        });
    }
}
