namespace EngageGovContentMcp.UnitTests.Domain;

using EngageGovContentMcp.Domain.Entities;
using Xunit;

public class ContentItemTests
{
    [Fact]
    public void Constructor_WithValidData_CreatesContentItem()
    {
        // Arrange
        var title = "Test Title";
        var description = "Test Description";
        var content = "Test Content";
        var category = "Test Category";

        // Act
        var contentItem = new ContentItem(title, description, content, category);

        // Assert
        Assert.Equal(title, contentItem.Title);
        Assert.Equal(description, contentItem.Description);
        Assert.Equal(content, contentItem.Content);
        Assert.Equal(category, contentItem.Category);
        Assert.False(contentItem.IsPublished);
        Assert.Null(contentItem.PublishedAt);
        Assert.NotEqual(Guid.Empty, contentItem.Id);
    }

    [Fact]
    public void Constructor_WithEmptyTitle_ThrowsArgumentException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => 
            new ContentItem(string.Empty, "desc", "content", "category"));
    }

    [Fact]
    public void Constructor_WithEmptyContent_ThrowsArgumentException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => 
            new ContentItem("title", "desc", string.Empty, "category"));
    }

    [Fact]
    public void Publish_UnpublishedContent_SetsPublishedState()
    {
        // Arrange
        var contentItem = new ContentItem("Title", "Desc", "Content", "Category");

        // Act
        contentItem.Publish();

        // Assert
        Assert.True(contentItem.IsPublished);
        Assert.NotNull(contentItem.PublishedAt);
    }

    [Fact]
    public void Unpublish_PublishedContent_SetsUnpublishedState()
    {
        // Arrange
        var contentItem = new ContentItem("Title", "Desc", "Content", "Category");
        contentItem.Publish();

        // Act
        contentItem.Unpublish();

        // Assert
        Assert.False(contentItem.IsPublished);
        Assert.Null(contentItem.PublishedAt);
    }

    [Fact]
    public void UpdateContent_WithValidData_UpdatesContentItem()
    {
        // Arrange
        var contentItem = new ContentItem("Old Title", "Old Desc", "Old Content", "Old Category");
        var newTitle = "New Title";
        var newDescription = "New Description";
        var newContent = "New Content";
        var newCategory = "New Category";

        // Act
        contentItem.UpdateContent(newTitle, newDescription, newContent, newCategory);

        // Assert
        Assert.Equal(newTitle, contentItem.Title);
        Assert.Equal(newDescription, contentItem.Description);
        Assert.Equal(newContent, contentItem.Content);
        Assert.Equal(newCategory, contentItem.Category);
    }

    [Fact]
    public void UpdateContent_WithEmptyTitle_ThrowsArgumentException()
    {
        // Arrange
        var contentItem = new ContentItem("Title", "Desc", "Content", "Category");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
            contentItem.UpdateContent(string.Empty, "desc", "content", "category"));
    }
}
