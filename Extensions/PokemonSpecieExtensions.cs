using remote_pokedex.Pokemons.Endpoints.Responses;
using remote_pokedex.Pokemons.Services.DTOs;

namespace remote_pokedex.Extensions;

public static class PokemonSpecieExtensions
{
    public static PokemonInfo MapPokemon(this PokemonSpecie specie)
    {
        return new PokemonInfo
        {
            Name = specie.name ?? string.Empty,
            Description = specie.flavor_text_entries?.FirstOrDefault(fte => fte.language.name.Equals("en"))?.flavor_text ?? string.Empty,
            Habitat = specie.habitat?.name ?? string.Empty,
            IsLegendary = specie.is_legendary ?? default
        };
    }
}
