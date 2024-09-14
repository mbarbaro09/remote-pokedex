using Microsoft.Extensions.Configuration;
using remote_pokedex.Repositories.DTO;
using System.Text.Json;

namespace remote_pokedex.Repositories;

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
