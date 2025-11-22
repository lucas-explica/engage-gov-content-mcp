namespace EngageGovContentMcp.Domain.Entities;

using EngageGovContentMcp.Domain.Common;

/// <summary>
/// Represents a content item in the system.
/// This is a sample entity demonstrating the domain model.
/// </summary>
public class ContentItem : BaseEntity
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string Content { get; private set; }
    public string Category { get; private set; }
    public bool IsPublished { get; private set; }
    public DateTime? PublishedAt { get; private set; }

    private ContentItem() : base()
    {
        Title = string.Empty;
        Description = string.Empty;
        Content = string.Empty;
        Category = string.Empty;
        IsPublished = false;
    }

    public ContentItem(string title, string description, string content, string category) : base()
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));

        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Content cannot be empty", nameof(content));

        Title = title;
        Description = description ?? string.Empty;
        Content = content;
        Category = category ?? string.Empty;
        IsPublished = false;
    }

    public void UpdateContent(string title, string description, string content, string category)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));

        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Content cannot be empty", nameof(content));

        Title = title;
        Description = description ?? string.Empty;
        Content = content;
        Category = category ?? string.Empty;
        UpdateTimestamp();
    }

    public void Publish()
    {
        if (!IsPublished)
        {
            IsPublished = true;
            PublishedAt = DateTime.UtcNow;
            UpdateTimestamp();
        }
    }

    public void Unpublish()
    {
        if (IsPublished)
        {
            IsPublished = false;
            PublishedAt = null;
            UpdateTimestamp();
        }
    }
}
