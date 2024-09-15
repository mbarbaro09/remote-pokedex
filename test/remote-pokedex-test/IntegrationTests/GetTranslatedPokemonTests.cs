using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using remote_pokedex.Pokemons.Endpoints.Responses;
using System.Net;
using System.Net.Http.Json;
using Xunit.Categories;

namespace remote_pokedex_test.IntegrationTests;

[IntegrationTest]
public class GetTranslatedPokemonTests
{
    [Fact]
    public async Task Get_ValidPokemonName_ReturnsHttpOk()
    {
        // Arrange
        string name = "pikachu";

        await using var webApplicationFactory = new WebApplicationFactory<Program>();

        using var httpClient = webApplicationFactory.CreateClient();

        // Act
        using var act = await httpClient.GetAsync($"/pokemon/translated/{name}");

        // Assert
        act.EnsureSuccessStatusCode();
        act.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await act.Content.ReadFromJsonAsync<PokemonInfo>();
        response.Should().NotBeNull();
        response.Name.Should().NotBeNullOrWhiteSpace();
        response.Description.Should().NotBeNullOrWhiteSpace();
        response.Habitat.Should().NotBeNullOrWhiteSpace();
        response.IsLegendary.Should().BeFalse();
    }

    [Fact]
    public async Task Get_InvalidPokemonName_ReturnsHttpNotFound()
    {
        // Arrange
        string name = "fake-pokemon-name!!##";

        await using var webApplicationFactory = new WebApplicationFactory<Program>();

        using var httpClient = webApplicationFactory.CreateClient();

        // Act
        using var act = await httpClient.GetAsync($"/pokemon/translated/{name}");

        // Assert
        act.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var response = await act.Content.ReadAsStringAsync();
        response.Should().Be("\"The pokemon could not be found, Try again later.\"");
    }

    [Theory]
    [InlineData("mewtwo", "Created by a scientist after years of horrific gene splicing and dna engineering experiments,  it was.")] //legendary pokemons should use Yoda translation
    [InlineData("diglett", "On plant roots,  lives about one yard underground where it feeds.Above ground,  it sometimes appears.")] //pokemons from habitat cave should use Yoda translation
    [InlineData("pikachu", "At which hour several of these pokémon gather,  their electricity couldst buildeth and cause lightning storms.")] //other pokemons should use Shakespeare translation
    public async Task Get_PokemonName_Should_ReturnCorrectTranslation(string name, string expected)
    {
        // Arrange
        await using var webApplicationFactory = new WebApplicationFactory<Program>();

        using var httpClient = webApplicationFactory.CreateClient();

        // Act
        using var act = await httpClient.GetAsync($"/pokemon/translated/{name}");

        // Assert
        act.EnsureSuccessStatusCode();
        act.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await act.Content.ReadFromJsonAsync<PokemonInfo>();
        response.Should().NotBeNull();
        response.Description.Should().Be(expected);
    }
}
