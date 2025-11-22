namespace EngageGovContentMcp.Infrastructure.Repositories;

using EngageGovContentMcp.Application.Interfaces;
using EngageGovContentMcp.Domain.Entities;
using EngageGovContentMcp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Repository implementation for ContentItem entity.
/// Provides data access using Entity Framework Core.
/// </summary>
public class ContentItemRepository : IContentItemRepository
{
    private readonly ApplicationDbContext _context;

    public ContentItemRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<ContentItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ContentItems
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<ContentItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ContentItems
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ContentItem>> GetPublishedAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ContentItems
            .Where(c => c.IsPublished)
            .OrderByDescending(c => c.PublishedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ContentItem>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default)
    {
        return await _context.ContentItems
            .Where(c => c.Category == category)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<ContentItem> AddAsync(ContentItem contentItem, CancellationToken cancellationToken = default)
    {
        await _context.ContentItems.AddAsync(contentItem, cancellationToken);
        return contentItem;
    }

    public Task UpdateAsync(ContentItem contentItem, CancellationToken cancellationToken = default)
    {
        _context.ContentItems.Update(contentItem);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var contentItem = await GetByIdAsync(id, cancellationToken);
        if (contentItem != null)
        {
            _context.ContentItems.Remove(contentItem);
        }
    }
}
