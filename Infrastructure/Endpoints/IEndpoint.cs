namespace remote_pokedex.Infrastructure.Endpoints;

public interface IEndpoint
{
    /// <summary>
    /// Exposes the endpoint action to the route
    /// </summary>
    /// <param name="app">The route builder where to specify the endpoint details</param>
    void MapEndpoint(IEndpointRouteBuilder app);
}
