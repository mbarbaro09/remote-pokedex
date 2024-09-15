using remote_pokedex.Pokemons.Services.DTOs;
using remote_pokedex.Pokemons.Extensions;
using Xunit.Categories;
using FluentAssertions;
using remote_pokedex.Pokemons.Endpoints.Responses;

namespace remote_pokedex_test.UnitTests;

[UnitTest]
public class PokemonSpecieExtensionsTests
{
    [Fact]
    public void MapPokemon_ShouldMapCorrectly_WhenValidSpecie()
    {
        // Arrange
        var specie = new PokemonSpecie(
            null, null, null, null, null, null, 
            flavor_text_entries: [new FlavorTextEntry("A yellow electric mouse", new Language("en", ""), new remote_pokedex.Pokemons.Services.DTOs.Version("1", ""))],
            null, null, null, null, null, null, 
            habitat: new Habitat("forest", ""),
            null, null, null, null,
            is_legendary: false,
            null, 
            name: "Pikachu", 
            null, null, null, null, null, null
        );

        // Act
        var result = specie.MapPokemon();

        // Assert
        result.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(new PokemonInfo
            {
                Name = "Pikachu",
                Description = "A yellow electric mouse",
                Habitat = "forest",
                IsLegendary = false
            });
    }

    [Fact]
    public void MapPokemon_ShouldReturnEmptyFields_WhenSpecieHasNullValues()
    {
        // Arrange
        var specie = new PokemonSpecie(
            null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null
        );

        // Act
        var result = specie.MapPokemon();

        // Assert
        result.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(new PokemonInfo
            {
                Name = string.Empty,
                Description = string.Empty,
                Habitat = string.Empty,
                IsLegendary = false //default
            });
    }

    [Fact]
    public void MapPokemon_ShouldReturnFirstEnglishDescription_WhenMultipleLanguagesExist()
    {
        // Arrange
        var specie = new PokemonSpecie(
            null, null, null, null, null, null,
            flavor_text_entries:
            [
                new FlavorTextEntry("Ein feuriger Drache", new Language("de", ""), new remote_pokedex.Pokemons.Services.DTOs.Version("1", "")),
                new FlavorTextEntry("A fiery dragon", new Language("en", ""), new remote_pokedex.Pokemons.Services.DTOs.Version("1", ""))
            ],
            null, null, null, null, null, null,
            habitat: new Habitat("mountain", ""),
            null, null, null, null,
            is_legendary: false,
            null,
            name: "Charizard",
            null, null, null, null, null, null
        );

        // Act
        var result = specie.MapPokemon();

        // Assert
        result.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(new PokemonInfo 
            { 
                Name = "Charizard", 
                Description = "A fiery dragon", 
                Habitat = "mountain", 
                IsLegendary = false 
            }, "First English Description");
    }
}
