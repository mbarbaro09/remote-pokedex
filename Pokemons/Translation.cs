using System.Text.Json.Serialization;

namespace remote_pokedex.Pokemons;

public record Contents(
    [property: JsonPropertyName("translated")] string translated,
    [property: JsonPropertyName("text")] string text,
    [property: JsonPropertyName("translation")] string translation
);

public record Translation(
    [property: JsonPropertyName("success")] Success success,
    [property: JsonPropertyName("contents")] Contents contents
);

public record Success(
    [property: JsonPropertyName("total")] int? total
);