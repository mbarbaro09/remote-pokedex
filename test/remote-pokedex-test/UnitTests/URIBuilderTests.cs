using FluentAssertions;
using remote_pokedex.Infrastructure.Endpoints;
using Xunit.Categories;


namespace remote_pokedex_test.UnitTests;

[UnitTest]
public class URIBuilderTests
{
    const string exampleBaseUrl = "http://www.test.com";

    [Theory]
    [InlineData(exampleBaseUrl, new string[] { }, new string[] { }, new string[] { }, $"{exampleBaseUrl}")]
    [InlineData(exampleBaseUrl, new string[] { "route1", "route2" }, new string[] { }, new string[] { }, $"{exampleBaseUrl}/route1/route2")]
    [InlineData(exampleBaseUrl, new string[] { "route" }, new string[] { "key" }, new string[] { "value" }, $"{exampleBaseUrl}/route?key=value")]
    [InlineData(exampleBaseUrl, new string[] { "route" }, new string[] { "key1", "key2" }, new string[] { "value1", "value2" }, $"{exampleBaseUrl}/route?key1=value1&key2=value2")]
    [InlineData(exampleBaseUrl, new string[] { }, new string[] { "key" }, new string[] { "x y\nz" }, $"{exampleBaseUrl}?key=x%20y%20z")]
    public void Should_ReturnCorrectUri(
        string baseUrl, 
        string[] routes, 
        string[] queryKeys, 
        string[] queryValues, 
        string expected
    ) {
        // Arrange
        URIBuilder builder = new(baseUrl);

        // Act
        foreach (var route in routes) builder.AddRoute(route);
        for (int i = 0; i < queryKeys.Length; i ++) builder.AddQueryParam(queryKeys[i], queryValues[i]);
        
        string url = builder.Build();

        // Assert
        url.Should().NotBeNullOrWhiteSpace().And.Be(expected);
    }

    [Fact]
    public void Should_ThrowArgumentNullException_When_BaseUrlIsNull()
    {
        // Arrange
        const string baseUrl = null;

        // Act
        var act = () => new URIBuilder(baseUrl);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Should_ThrowArgumentException_When_BaseUrlIsEmpty()
    {
        // Arrange
        string baseUrl = string.Empty;

        // Act
        var act = () => new URIBuilder(baseUrl);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Should_ThrowArgumentNullException_When_RouteIsNull()
    {
        // Arrange
        var builder = new URIBuilder(exampleBaseUrl);
        string route = null;

        // Act
        var act = () => builder.AddRoute(route);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Should_ThrowArgumentException_When_RouteIsEmpty()
    {
        // Arrange
        var builder = new URIBuilder(exampleBaseUrl);
        string route = string.Empty;

        // Act
        var act = () => builder.AddRoute(route);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Should_ThrowArgumentNullException_When_QueryParamIsNull()
    {
        // Arrange
        var builder = new URIBuilder(exampleBaseUrl);
        string key = "key";
        string value = "value";

        // Act
        var actKey = () => builder.AddQueryParam(null, value);
        var actValue = () => builder.AddQueryParam(key, null);

        // Assert
        actKey.Should().Throw<ArgumentNullException>();
        actValue.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Should_ThrowArgumentException_When_QueryParamIsEmpty()
    {
        // Arrange
        var builder = new URIBuilder(exampleBaseUrl);
        string key = "key";
        string value = "value";

        // Act
        var actKey = () => builder.AddQueryParam(string.Empty, value);
        var actValue = () => builder.AddQueryParam(key, string.Empty);

        // Assert
        actKey.Should().Throw<ArgumentException>();
        actValue.Should().Throw<ArgumentException>();
    }
}
