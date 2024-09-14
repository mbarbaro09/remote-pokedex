using System.Text.Json.Serialization;

namespace remote_pokedex.Repositories.DTO;

public record Area(
    [property: JsonPropertyName("name")] string name,
    [property: JsonPropertyName("url")] string url
);

public record Color(
    [property: JsonPropertyName("name")] string name,
    [property: JsonPropertyName("url")] string url
);

public record EggGroup(
    [property: JsonPropertyName("name")] string name,
    [property: JsonPropertyName("url")] string url
);

public record EvolutionChain(
    [property: JsonPropertyName("url")] string url
);

public record FlavorTextEntry(
    [property: JsonPropertyName("flavor_text")] string flavor_text,
    [property: JsonPropertyName("language")] Language language,
    [property: JsonPropertyName("version")] Version version
);

public record Genera(
    [property: JsonPropertyName("genus")] string genus,
    [property: JsonPropertyName("language")] Language language
);

public record Generation(
    [property: JsonPropertyName("name")] string name,
    [property: JsonPropertyName("url")] string url
);

public record GrowthRate(
    [property: JsonPropertyName("name")] string name,
    [property: JsonPropertyName("url")] string url
);

public record Habitat(
    [property: JsonPropertyName("name")] string name,
    [property: JsonPropertyName("url")] string url
);

public record Language(
    [property: JsonPropertyName("name")] string name,
    [property: JsonPropertyName("url")] string url
);

public record Name(
    [property: JsonPropertyName("language")] Language language,
    [property: JsonPropertyName("name")] string name
);

public record PalParkEncounter(
    [property: JsonPropertyName("area")] Area area,
    [property: JsonPropertyName("base_score")] int? base_score,
    [property: JsonPropertyName("rate")] int? rate
);

public record Pokedex(
    [property: JsonPropertyName("name")] string name,
    [property: JsonPropertyName("url")] string url
);

public record PokedexNumber(
    [property: JsonPropertyName("entry_number")] int? entry_number,
    [property: JsonPropertyName("pokedex")] Pokedex pokedex
);

public record Pokemon(
    [property: JsonPropertyName("name")] string name,
    [property: JsonPropertyName("url")] string url
);

public record PokemonSpecie(
    [property: JsonPropertyName("base_happiness")] int? base_happiness,
    [property: JsonPropertyName("capture_rate")] int? capture_rate,
    [property: JsonPropertyName("color")] Color color,
    [property: JsonPropertyName("egg_groups")] IReadOnlyList<EggGroup> egg_groups,
    [property: JsonPropertyName("evolution_chain")] EvolutionChain evolution_chain,
    [property: JsonPropertyName("evolves_from_species")] object evolves_from_species,
    [property: JsonPropertyName("flavor_text_entries")] IReadOnlyList<FlavorTextEntry> flavor_text_entries,
    [property: JsonPropertyName("form_descriptions")] IReadOnlyList<object> form_descriptions,
    [property: JsonPropertyName("forms_switchable")] bool? forms_switchable,
    [property: JsonPropertyName("gender_rate")] int? gender_rate,
    [property: JsonPropertyName("genera")] IReadOnlyList<Genera> genera,
    [property: JsonPropertyName("generation")] Generation generation,
    [property: JsonPropertyName("growth_rate")] GrowthRate growth_rate,
    [property: JsonPropertyName("habitat")] Habitat habitat,
    [property: JsonPropertyName("has_gender_differences")] bool? has_gender_differences,
    [property: JsonPropertyName("hatch_counter")] int? hatch_counter,
    [property: JsonPropertyName("id")] int? id,
    [property: JsonPropertyName("is_baby")] bool? is_baby,
    [property: JsonPropertyName("is_legendary")] bool? is_legendary,
    [property: JsonPropertyName("is_mythical")] bool? is_mythical,
    [property: JsonPropertyName("name")] string name,
    [property: JsonPropertyName("names")] IReadOnlyList<Name> names,
    [property: JsonPropertyName("order")] int? order,
    [property: JsonPropertyName("pal_park_encounters")] IReadOnlyList<PalParkEncounter> pal_park_encounters,
    [property: JsonPropertyName("pokedex_numbers")] IReadOnlyList<PokedexNumber> pokedex_numbers,
    [property: JsonPropertyName("shape")] Shape shape,
    [property: JsonPropertyName("varieties")] IReadOnlyList<Variety> varieties
);

public record Shape(
    [property: JsonPropertyName("name")] string name,
    [property: JsonPropertyName("url")] string url
);

public record Variety(
    [property: JsonPropertyName("is_default")] bool? is_default,
    [property: JsonPropertyName("pokemon")] Pokemon pokemon
);

public record Version(
    [property: JsonPropertyName("name")] string name,
    [property: JsonPropertyName("url")] string url
);

