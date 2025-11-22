namespace EngageGovContentMcp.Application.Interfaces;

using EngageGovContentMcp.Domain.Entities;

/// <summary>
/// Repository interface for ContentItem entity.
/// Follows Repository pattern for data access abstraction.
/// </summary>
public interface IContentItemRepository
{
    Task<ContentItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ContentItem>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ContentItem>> GetPublishedAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ContentItem>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default);
    Task<ContentItem> AddAsync(ContentItem contentItem, CancellationToken cancellationToken = default);
    Task UpdateAsync(ContentItem contentItem, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
