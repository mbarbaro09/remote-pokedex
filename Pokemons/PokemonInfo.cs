using System.Text.Json.Serialization;

namespace remote_pokedex.Pokemons
{
    /// <summary>
    /// Pokemon data returned to the API consumer
    /// </summary>
    public class PokemonInfo
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
        
        [JsonPropertyName("habitat")]
        public string Habitat { get; set; }
        
        [JsonPropertyName("is_legendary")]
        public bool IsLegendary { get; set; }
    }
}
