using remote_pokedex.Infrastructure.Services;
using remote_pokedex.Pokemons.Services.DTOs;

namespace remote_pokedex.Pokemons.Services;

public interface IPokeAPIRepository
{
    Task<PokemonSpecie> GetPokemonSpecie(string name);
}

/// <summary>
/// Repository that retrieves pokemon information from the data source via REST API
/// </summary>
public class PokeAPIRepository(IConfiguration configuration, ILogger<PokeAPIRepository> logger) 
    : BaseClient(configuration.GetSection("ServicesURL").GetValue<string>("PokeAPI"), logger), IPokeAPIRepository
{
    public async Task<PokemonSpecie> GetPokemonSpecie(string name)
    {
        return await GetAsync<PokemonSpecie>($"pokemon-species/{name}");
    }
}
