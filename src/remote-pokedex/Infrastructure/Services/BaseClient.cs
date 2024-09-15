using remote_pokedex.Infrastructure.Endpoints;
using remote_pokedex.Infrastructure.Exceptions;
using System.Text.Json;

namespace remote_pokedex.Infrastructure.Services
{
    public abstract class BaseClient(string baseUrl, ILogger<BaseClient> logger) : IDisposable
    {
        private readonly ILogger<BaseClient> logger = logger;
        private readonly HttpClient _httpClient = new();
        private readonly string _baseUrl = baseUrl;

        public async Task<T> GetAsync<T>(string route) where T : class
        {
            string url = GetUrl(route);
            HttpResponseMessage? response;

            try
            {
                logger.LogDebug("[GET] Calling: {url}", url);
                response = await _httpClient.GetAsync(url);
            }
            catch (Exception ex)
            {
                throw LogAndThrow($"Error while calling: {url}", HttpResponseErrorType.FAILED, ex);
            }

            return await HandleResponse<T>(response);
        }

        public async Task<T> PostAsync<T>(
            string route, 
            IEnumerable<(string, string)>? routeParams = null, 
            object? body = null
        ) where T : class 
        {
            string url = GetUrl(route, routeParams);

            HttpResponseMessage? response;
            try
            {
                logger.LogDebug("[POST] Calling: {url}", url);
                response = await _httpClient.PostAsJsonAsync(url, JsonSerializer.Serialize(body));
            }
            catch (Exception ex)
            {
                throw LogAndThrow($"Error while calling: {url}", HttpResponseErrorType.FAILED, ex);
            }

            return await HandleResponse<T>(response);
        }

        private async Task<T> HandleResponse<T>(HttpResponseMessage response) where T : class
        {
            if (response is null || !response.IsSuccessStatusCode)
                throw LogAndThrow($"Response was empty", HttpResponseErrorType.EMPTY);

            if (!response.IsSuccessStatusCode)
                throw LogAndThrow($"Response was not successful: {response?.StatusCode}", HttpResponseErrorType.FAILED);

            var stream = await response.Content.ReadAsStreamAsync();

            if (stream.Length <= 0)
                throw LogAndThrow($"Response body was empty and not deserializable into {nameof(T)}.", HttpResponseErrorType.EMPTY);

            T? resource = null;
            try
            {
                resource = await JsonSerializer.DeserializeAsync<T>(stream);
            }
            catch (Exception ex) when (ex is JsonException or ArgumentNullException)
            {
                throw LogAndThrow($"Error while deserializing response to {nameof(T)}. Response: {response.Content?.ToString() ?? ""}", HttpResponseErrorType.FAILED, ex);
            }

            return resource ?? throw new HttpClientException("Response was empty", HttpResponseErrorType.EMPTY);
        }

        private string GetUrl(string route, IEnumerable<(string, string)>? routeParams = null)
        {
            URIBuilder uri = new(_baseUrl);
            uri.AddRoute(route);

            if (routeParams is not null && routeParams.Any())
            {
                foreach (var routeParam in routeParams)
                    uri.AddQueryParam(routeParam.Item1, routeParam.Item2);
            }

            return uri.Build();
        }

        private HttpClientException LogAndThrow(string error, HttpResponseErrorType type, Exception? ex = null)
        {
            logger.LogError(error);
            return new HttpClientException(error, type, ex);
        }

        public void Dispose() => _httpClient.Dispose();
    }
}
