namespace EngageGovContentMcp.Application.Interfaces;

/// <summary>
/// Unit of Work interface for managing database transactions.
/// Ensures atomicity of operations across multiple repositories.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IContentItemRepository ContentItems { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
