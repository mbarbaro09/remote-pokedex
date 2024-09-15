using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace remote_pokedex.Infrastructure.Endpoints;

/// <summary>
/// Extension class to abstract the instantiation of endpoints based on IEndpoint interface
/// </summary>
public static class EndpointExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services)
    {
        ServiceDescriptor[] serviceDescriptors = Assembly.GetExecutingAssembly()
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } 
                           && type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptors);

        return services;
    }

    public static IApplicationBuilder MapEndpoints(this WebApplication app, RouteGroupBuilder? routeGroupBuilder = null)
    {
        IEnumerable<IEndpoint> endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        IEndpointRouteBuilder builder = routeGroupBuilder is null ? app : routeGroupBuilder;

        foreach (IEndpoint endpoint in endpoints)
            endpoint.MapEndpoint(builder);

        return app;
    }
}
