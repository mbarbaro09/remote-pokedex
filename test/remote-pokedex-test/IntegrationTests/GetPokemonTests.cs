using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;
using System.Net;
using Xunit.Categories;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;
using remote_pokedex.Pokemons.Endpoints.Responses;

namespace remote_pokedex_test.IntegrationTests;

[IntegrationTest]
public class GetPokemonTests
{
    [Fact]
    public async Task Get_ValidPokemonName_ReturnsHttpOk()
    {
        // Arrange
        string name = "pikachu";

        await using var webApplicationFactory = new WebApplicationFactory<Program>();

        using var httpClient = webApplicationFactory.CreateClient();

        // Act
        using var act = await httpClient.GetAsync($"/pokemon/{name}");

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
        using var act = await httpClient.GetAsync($"/pokemon/{name}");

        // Assert
        act.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var response = await act.Content.ReadAsStringAsync();
        response.Should().Be("\"The pokemon could not be found, Try again later.\"");
    }
}
