namespace EngageGovContentMcp.Infrastructure;

using EngageGovContentMcp.Application.Interfaces;
using EngageGovContentMcp.Infrastructure.Data;
using EngageGovContentMcp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Dependency injection configuration for infrastructure layer.
/// Registers repositories and database context.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, bool useInMemoryDatabase = true)
    {
        // Register DbContext
        if (useInMemoryDatabase)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("EngageGovContentMcpDb"));
        }

        // Register repositories
        services.AddScoped<IContentItemRepository, ContentItemRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
