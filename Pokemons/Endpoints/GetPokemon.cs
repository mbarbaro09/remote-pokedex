using remote_pokedex.Infrastructure.Endpoints;
using remote_pokedex.Infrastructure.Exceptions;
using remote_pokedex.Pokemons.Endpoints.Responses;
using remote_pokedex.Pokemons.Extensions;
using remote_pokedex.Pokemons.Services;
using remote_pokedex.Pokemons.Services.DTOs;

namespace remote_pokedex.Pokemons.Endpoints;

public static class GetPokemon
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("pokemon/{name}", Handler)
               .WithDescription("Given a Pokemon name, returns standard Pokemon description and additional information.")
               .WithTags("Pokemon")
               .AllowAnonymous()
               .Produces(StatusCodes.Status200OK, typeof(PokemonInfo))
               .Produces(StatusCodes.Status400BadRequest)
               .Produces(StatusCodes.Status404NotFound)
               .Produces(StatusCodes.Status500InternalServerError);
        }
    }

    public static async Task<IResult> Handler(string name, IPokeAPIRepository pokeAPIRepository)
    {
        if (string.IsNullOrWhiteSpace(name))
            return  Results.BadRequest("The request was not formatted correctly! Pokemon name is missing or empty.");

        PokemonSpecie specie;
        try
        {
            specie = await pokeAPIRepository.GetPokemonSpecie(name);
        }
        catch (HttpClientException ex)
        {
            return ex.ErrorType switch
            {
                HttpResponseErrorType.FAILED => Results.Problem(statusCode: StatusCodes.Status500InternalServerError, detail: "The request failed for an internal error. Try again later!"),
                HttpResponseErrorType.EMPTY => Results.NotFound("The pokemon could not be found, Try again later."),
                _ => throw new NotImplementedException()
            };
        }

        return Results.Ok(specie.MapPokemon());
    }
}
