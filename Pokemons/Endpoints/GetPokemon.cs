using Microsoft.AspNetCore.Http.HttpResults;
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
               .WithTags("Pokemon")
               .WithDescription("Given a Pokemon name, returns standard Pokemon description and additional information.");
        }
    }

    public static async Task<Results<Ok<PokemonInfo>, BadRequest<string>, NotFound<string>>> Handler(string name, IPokeAPIRepository pokeAPIRepository)
    {
        if (string.IsNullOrWhiteSpace(name))
            return TypedResults.BadRequest("The request was not formatted correctly! Pokemon name is missing or empty.");

        PokemonSpecie specie;
        try
        {
            specie = await pokeAPIRepository.GetPokemonSpecie(name);
        }
        catch (HttpClientException)
        {
            return TypedResults.NotFound("The pokemon could not be found, Try again later.");
        }

        return TypedResults.Ok(specie.MapPokemon());
    }
}
