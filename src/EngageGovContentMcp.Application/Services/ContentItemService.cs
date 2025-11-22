namespace EngageGovContentMcp.Application.Services;

using EngageGovContentMcp.Application.DTOs;
using EngageGovContentMcp.Application.Interfaces;
using EngageGovContentMcp.Domain.Entities;

/// <summary>
/// Service for managing content items.
/// Implements business logic and orchestrates domain operations.
/// Follows Single Responsibility Principle.
/// </summary>
public class ContentItemService : IContentItemService
{
    private readonly IUnitOfWork _unitOfWork;

    public ContentItemService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<ContentItemDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var contentItem = await _unitOfWork.ContentItems.GetByIdAsync(id, cancellationToken);
        return contentItem != null ? MapToDto(contentItem) : null;
    }

    public async Task<IEnumerable<ContentItemDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var contentItems = await _unitOfWork.ContentItems.GetAllAsync(cancellationToken);
        return contentItems.Select(MapToDto);
    }

    public async Task<IEnumerable<ContentItemDto>> GetPublishedAsync(CancellationToken cancellationToken = default)
    {
        var contentItems = await _unitOfWork.ContentItems.GetPublishedAsync(cancellationToken);
        return contentItems.Select(MapToDto);
    }

    public async Task<IEnumerable<ContentItemDto>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default)
    {
        var contentItems = await _unitOfWork.ContentItems.GetByCategoryAsync(category, cancellationToken);
        return contentItems.Select(MapToDto);
    }

    public async Task<ContentItemDto> CreateAsync(CreateContentItemDto dto, CancellationToken cancellationToken = default)
    {
        var contentItem = new ContentItem(dto.Title, dto.Description, dto.Content, dto.Category);
        var created = await _unitOfWork.ContentItems.AddAsync(contentItem, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return MapToDto(created);
    }

    public async Task<ContentItemDto?> UpdateAsync(Guid id, UpdateContentItemDto dto, CancellationToken cancellationToken = default)
    {
        var contentItem = await _unitOfWork.ContentItems.GetByIdAsync(id, cancellationToken);
        if (contentItem == null)
            return null;

        contentItem.UpdateContent(dto.Title, dto.Description, dto.Content, dto.Category);
        await _unitOfWork.ContentItems.UpdateAsync(contentItem, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return MapToDto(contentItem);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var contentItem = await _unitOfWork.ContentItems.GetByIdAsync(id, cancellationToken);
        if (contentItem == null)
            return false;

        await _unitOfWork.ContentItems.DeleteAsync(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<ContentItemDto?> PublishAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var contentItem = await _unitOfWork.ContentItems.GetByIdAsync(id, cancellationToken);
        if (contentItem == null)
            return null;

        contentItem.Publish();
        await _unitOfWork.ContentItems.UpdateAsync(contentItem, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return MapToDto(contentItem);
    }

    public async Task<ContentItemDto?> UnpublishAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var contentItem = await _unitOfWork.ContentItems.GetByIdAsync(id, cancellationToken);
        if (contentItem == null)
            return null;

        contentItem.Unpublish();
        await _unitOfWork.ContentItems.UpdateAsync(contentItem, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return MapToDto(contentItem);
    }

    private static ContentItemDto MapToDto(ContentItem contentItem)
    {
        return new ContentItemDto
        {
            Id = contentItem.Id,
            Title = contentItem.Title,
            Description = contentItem.Description,
            Content = contentItem.Content,
            Category = contentItem.Category,
            IsPublished = contentItem.IsPublished,
            PublishedAt = contentItem.PublishedAt,
            CreatedAt = contentItem.CreatedAt,
            UpdatedAt = contentItem.UpdatedAt
        };
    }
}
