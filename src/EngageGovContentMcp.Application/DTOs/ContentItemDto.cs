namespace EngageGovContentMcp.Application.DTOs;

/// <summary>
/// Data Transfer Object for ContentItem.
/// Used to transfer data between layers without exposing domain entities.
/// </summary>
public class ContentItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public bool IsPublished { get; set; }
    public DateTime? PublishedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
