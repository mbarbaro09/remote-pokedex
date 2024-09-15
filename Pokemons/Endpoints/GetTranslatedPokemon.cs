using remote_pokedex.Infrastructure.Endpoints;
using remote_pokedex.Infrastructure.Exceptions;
using remote_pokedex.Pokemons.Endpoints.Responses;
using remote_pokedex.Pokemons.Services.DTOs;
using remote_pokedex.Pokemons.Services;
using remote_pokedex.Pokemons.Extensions;

namespace remote_pokedex.Pokemons.Endpoints;

public static class GetTranslatedPokemon
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("pokemon/translated/{name}", Handler)
               .WithDescription("Given a Pokemon name, return translated Pokemon description and other basic information")
               .WithTags("Pokemon")
               .AllowAnonymous()
               .Produces(StatusCodes.Status200OK, typeof(PokemonInfo))
               .Produces(StatusCodes.Status400BadRequest)
               .Produces(StatusCodes.Status404NotFound)
               .Produces(StatusCodes.Status500InternalServerError);
        }
    }

    public static async Task<IResult> Handler(
        string name, 
        IPokeAPIRepository pokeAPIRepository, 
        IFunTranslationsService translationsService
    ) {
        if (string.IsNullOrWhiteSpace(name))
            return Results.BadRequest("The request was not formatted correctly! Pokemon name is missing or empty.");

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

        PokemonInfo pokemon = specie.MapPokemon();

        try
        {
            pokemon.Description = pokemon.Habitat == "cave" || pokemon.IsLegendary
                ? await translationsService.GetYodaTranslation(pokemon.Description)
                : await translationsService.GetShakespeareTranslation(pokemon.Description);
        }
        catch (HttpClientException) 
        {
            // if description can't be translated for whatever reason we keep the standard translation.
        }

        return TypedResults.Ok(pokemon);
    }
}
