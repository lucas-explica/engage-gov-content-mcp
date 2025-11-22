namespace EngageGovContentMcp.Application.DTOs;

/// <summary>
/// DTO for updating an existing content item.
/// </summary>
public class UpdateContentItemDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
}
