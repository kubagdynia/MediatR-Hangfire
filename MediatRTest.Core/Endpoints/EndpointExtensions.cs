using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MediatRTest.Core.Endpoints;

public static class EndpointExtensions
{
    /// <summary>
    /// Scan assemblies and registers endpoints
    /// </summary>
    /// <param name="services">IServiceCollection</param>
    /// <param name="assemblies">Assemblies to scan for endpoints</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly[] assemblies)
    {

        if (assemblies.Length == 0)
            throw new ArgumentException(
                "No assemblies found to scan. Supply at least one assembly to scan for endpoints.");

        // Register all endpoints
        var serviceDescriptors = assemblies.Distinct()
            .SelectMany(a => a.DefinedTypes)
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                           type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();
        
        // Add all endpoints to the service collection
        services.TryAddEnumerable(serviceDescriptors);
        return services;
    }

    /// <summary>
    /// Scan assembly and registers endpoints
    /// </summary>
    /// <param name="services">IServiceCollection</param>
    /// <param name="assembly">Assembly to scan for endpoints</param>
    /// <returns></returns>
    public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
        => AddEndpoints(services, [assembly]);

    /// <summary>
    /// Scan executing assembly and registers endpoints. 
    /// </summary>
    public static IApplicationBuilder MapEndpoints(this WebApplication app, IEndpointRouteBuilder? routeGroupBuilder = null)
    {
        // Get all endpoints
        var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();
        
        var builder = routeGroupBuilder ?? app;

        // Map all endpoints
        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoint(builder);
        }

        return app;
    }
}