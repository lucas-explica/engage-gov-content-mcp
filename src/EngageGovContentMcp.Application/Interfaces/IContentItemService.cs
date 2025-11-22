namespace EngageGovContentMcp.Application.Interfaces;

using EngageGovContentMcp.Application.DTOs;

/// <summary>
/// Interface for ContentItem service operations.
/// Defines the contract for content management operations.
/// </summary>
public interface IContentItemService
{
    Task<ContentItemDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ContentItemDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ContentItemDto>> GetPublishedAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ContentItemDto>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default);
    Task<ContentItemDto> CreateAsync(CreateContentItemDto dto, CancellationToken cancellationToken = default);
    Task<ContentItemDto?> UpdateAsync(Guid id, UpdateContentItemDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ContentItemDto?> PublishAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ContentItemDto?> UnpublishAsync(Guid id, CancellationToken cancellationToken = default);
}
