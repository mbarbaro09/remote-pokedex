using remote_pokedex.Infrastructure.Services;

namespace remote_pokedex.Pokemons;

public interface IPokeAPIRepository
{
    Task<PokemonSpecie> GetPokemonSpecie(string name);
}

public class PokeAPIRepository(string baseUrl) : BaseClient(baseUrl), IPokeAPIRepository
{
    public async Task<PokemonSpecie> GetPokemonSpecie(string name)
    {
        return await GetAsync<PokemonSpecie>($"pokemon-species/{name}");
    }
}
