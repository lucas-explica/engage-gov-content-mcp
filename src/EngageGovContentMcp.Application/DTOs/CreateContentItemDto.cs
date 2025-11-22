namespace EngageGovContentMcp.Application.DTOs;

/// <summary>
/// DTO for creating a new content item.
/// </summary>
public class CreateContentItemDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
}
