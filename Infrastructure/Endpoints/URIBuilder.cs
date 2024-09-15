namespace remote_pokedex.Infrastructure.Endpoints
{
    public class URIBuilder(string baseUrl)
    {
        private readonly string _base = baseUrl;
        private readonly List<string> _routes = [];
        private readonly List<string> _queryParams = [];

        public URIBuilder AddRoute(string route)
        {
            _routes.Add(route);
            return this;
        }

        public URIBuilder AddQueryParam(string key, string value)
        {
            _queryParams.Add(key + "=" + Uri.EscapeDataString(value.Replace("\n", " ")));
            return this;
        }

        public string Build()
        {
            if (string.IsNullOrEmpty(_base)) 
                return string.Empty;

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
}
