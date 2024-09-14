using Microsoft.Extensions.Configuration;
using remote_pokedex.Infrastructure.Services;
using System.Text.Json;

namespace remote_pokedex.Pokemons;

public interface IPokeAPIRepository
{
    Task<PokemonSpecie> GetPokemonSpecie(string name);
}

public class PokeAPIRepository(string baseUrl) : BaseClient(baseUrl), IPokeAPIRepository
{
    public async Task<PokemonSpecie> GetPokemonSpecie(string name)
    {
        var pokemon = await GetAsync<PokemonSpecie>($"pokemon-species/{name}");
        return pokemon;
    }
}
