using EngageGovContentMcp.Application;
using EngageGovContentMcp.Infrastructure;
using EngageGovContentMcp.Server.Middleware;
using Serilog;
using System.Text.Json.Serialization;

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
        .Build())
    .CreateLogger();

try
{
    Log.Information("Starting EngageGov Content MCP Server");

    var builder = WebApplication.CreateBuilder(args);

    // Add Serilog
    builder.Host.UseSerilog();

    // Add services to the container
    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

    // Add API documentation
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddOpenApi();

    // Add health checks
    builder.Services.AddHealthChecks();

    // Add CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowedOrigins", policy =>
        {
            var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() 
                ?? new[] { "http://localhost:3000" };
            
            policy.WithOrigins(allowedOrigins)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
    });

    // Add Application and Infrastructure layers
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(useInMemoryDatabase: true);

    // Configure Kestrel for security
    builder.WebHost.ConfigureKestrel(serverOptions =>
    {
        serverOptions.AddServerHeader = false;
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline
    
    // Security headers middleware (always first)
    app.UseMiddleware<SecurityHeadersMiddleware>();

    // Exception handling middleware
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    // Enable CORS
    app.UseCors("AllowedOrigins");

    // Enable HTTPS redirection
    if (!app.Environment.IsDevelopment())
    {
        app.UseHsts();
    }
    app.UseHttpsRedirection();

    // Enable API documentation in Development
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    // Add Serilog request logging
    app.UseSerilogRequestLogging();

    // Map health check endpoint
    app.MapHealthChecks("/health");

    // Map controllers
    app.MapControllers();

    // Root endpoint
    app.MapGet("/", () => new
    {
        service = "EngageGov Content MCP Server",
        version = "1.0.0",
        status = "running",
        timestamp = DateTime.UtcNow
    })
    .WithName("GetServiceInfo")
    .WithOpenApi();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

