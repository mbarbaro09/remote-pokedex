namespace remote_pokedex.Infrastructure.Endpoints;

public class URIBuilder
{
    private readonly string _base;
    private readonly List<string> _routes = [];
    private readonly List<string> _queryParams = [];

    public URIBuilder(string baseUrl)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(baseUrl, nameof(baseUrl));
        _base = baseUrl;
    }

    public URIBuilder AddRoute(string route)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(route, nameof(route));
        _routes.Add(route);
        return this;
    }

    public URIBuilder AddQueryParam(string key, string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key, nameof(key));
        ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));
        _queryParams.Add(key + "=" + Uri.EscapeDataString(value.Replace("\n", " ")));
        return this;
    }

    public string Build()
    {
        string uri = _base;

        if (uri.Last() == '/')
            uri = uri[..^1];

        foreach (var route in _routes)
            uri += "/" + route;

        if (_queryParams.Count > 0)
            uri += "?" + string.Join('&', _queryParams);

        return uri;
    }
}
