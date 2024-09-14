﻿using Microsoft.AspNetCore.Http.HttpResults;
using remote_pokedex.Infrastructure.Endpoints;
using remote_pokedex.Infrastructure.Exceptions;
using remote_pokedex.Pokemons.Endpoints.Responses;
using remote_pokedex.Pokemons.Services.DTOs;
using remote_pokedex.Pokemons.Services;
using remote_pokedex.Extensions;

namespace remote_pokedex.Pokemons.Endpoints;

public static class GetTranslatedPokemon
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("pokemon/translated/{name}", Handler)
               .WithTags("Pokemon");
        }
    }

    public static async Task<Results<Ok<PokemonInfo>, BadRequest<string>, NotFound<string>>> Handler(
        string name, 
        IPokeAPIRepository pokeAPIRepository, 
        IFunTranslationsService translationsService
    ) {
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
