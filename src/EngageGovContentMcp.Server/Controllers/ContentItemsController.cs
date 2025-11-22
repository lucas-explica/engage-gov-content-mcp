namespace EngageGovContentMcp.Server.Controllers;

using EngageGovContentMcp.Application.DTOs;
using EngageGovContentMcp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Controller for managing content items.
/// Provides RESTful API endpoints for CRUD operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ContentItemsController : ControllerBase
{
    private readonly IContentItemService _contentItemService;
    private readonly ILogger<ContentItemsController> _logger;

    public ContentItemsController(
        IContentItemService contentItemService,
        ILogger<ContentItemsController> logger)
    {
        _contentItemService = contentItemService ?? throw new ArgumentNullException(nameof(contentItemService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets all content items.
    /// </summary>
    /// <returns>List of content items.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ContentItemDto>>> GetAll(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all content items");
        var items = await _contentItemService.GetAllAsync(cancellationToken);
        return Ok(items);
    }

    /// <summary>
    /// Gets a specific content item by ID.
    /// </summary>
    /// <param name="id">The content item ID.</param>
    /// <returns>The content item if found.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContentItemDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting content item with ID: {Id}", id);
        var item = await _contentItemService.GetByIdAsync(id, cancellationToken);
        
        if (item == null)
        {
            _logger.LogWarning("Content item with ID {Id} not found", id);
            return NotFound();
        }

        return Ok(item);
    }

    /// <summary>
    /// Gets all published content items.
    /// </summary>
    /// <returns>List of published content items.</returns>
    [HttpGet("published")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ContentItemDto>>> GetPublished(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting published content items");
        var items = await _contentItemService.GetPublishedAsync(cancellationToken);
        return Ok(items);
    }

    /// <summary>
    /// Gets content items by category.
    /// </summary>
    /// <param name="category">The category name.</param>
    /// <returns>List of content items in the specified category.</returns>
    [HttpGet("category/{category}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ContentItemDto>>> GetByCategory(string category, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting content items for category: {Category}", category);
        var items = await _contentItemService.GetByCategoryAsync(category, cancellationToken);
        return Ok(items);
    }

    /// <summary>
    /// Creates a new content item.
    /// </summary>
    /// <param name="dto">The content item data.</param>
    /// <returns>The created content item.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ContentItemDto>> Create([FromBody] CreateContentItemDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _logger.LogInformation("Creating new content item: {Title}", dto.Title);

        try
        {
            var item = await _contentItemService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Error creating content item");
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Updates an existing content item.
    /// </summary>
    /// <param name="id">The content item ID.</param>
    /// <param name="dto">The updated content item data.</param>
    /// <returns>The updated content item.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContentItemDto>> Update(Guid id, [FromBody] UpdateContentItemDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _logger.LogInformation("Updating content item with ID: {Id}", id);

        try
        {
            var item = await _contentItemService.UpdateAsync(id, dto, cancellationToken);
            
            if (item == null)
            {
                _logger.LogWarning("Content item with ID {Id} not found", id);
                return NotFound();
            }

            return Ok(item);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Error updating content item");
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Deletes a content item.
    /// </summary>
    /// <param name="id">The content item ID.</param>
    /// <returns>No content if successful.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting content item with ID: {Id}", id);
        var success = await _contentItemService.DeleteAsync(id, cancellationToken);
        
        if (!success)
        {
            _logger.LogWarning("Content item with ID {Id} not found", id);
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Publishes a content item.
    /// </summary>
    /// <param name="id">The content item ID.</param>
    /// <returns>The published content item.</returns>
    [HttpPost("{id}/publish")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContentItemDto>> Publish(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Publishing content item with ID: {Id}", id);
        var item = await _contentItemService.PublishAsync(id, cancellationToken);
        
        if (item == null)
        {
            _logger.LogWarning("Content item with ID {Id} not found", id);
            return NotFound();
        }

        return Ok(item);
    }

    /// <summary>
    /// Unpublishes a content item.
    /// </summary>
    /// <param name="id">The content item ID.</param>
    /// <returns>The unpublished content item.</returns>
    [HttpPost("{id}/unpublish")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContentItemDto>> Unpublish(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Unpublishing content item with ID: {Id}", id);
        var item = await _contentItemService.UnpublishAsync(id, cancellationToken);
        
        if (item == null)
        {
            _logger.LogWarning("Content item with ID {Id} not found", id);
            return NotFound();
        }

        return Ok(item);
    }
}
