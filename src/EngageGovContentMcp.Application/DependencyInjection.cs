namespace EngageGovContentMcp.Application;

using EngageGovContentMcp.Application.Interfaces;
using EngageGovContentMcp.Application.Services;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Dependency injection configuration for application layer.
/// Registers application services.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register services
        services.AddScoped<IContentItemService, ContentItemService>();

        return services;
    }
}
