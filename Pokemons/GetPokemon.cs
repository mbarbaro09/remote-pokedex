using Microsoft.AspNetCore.Http.HttpResults;
using remote_pokedex.Extensions;
using remote_pokedex.Infrastructure.Endpoints;
using remote_pokedex.Infrastructure.Exceptions;
using remote_pokedex.Repositories;
using remote_pokedex.Repositories.DTO;

namespace remote_pokedex.Pokemons;

public static class GetPokemon
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("pokemon/{name}", Handler)
               .WithTags("Pokemon");
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
        catch (HttpClientException ex) 
        {
            return TypedResults.NotFound("The pokemon could not be found, Try again later.");
        }

        return TypedResults.Ok(specie.MapPokemon());
    }
}
