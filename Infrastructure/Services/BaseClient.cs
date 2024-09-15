using remote_pokedex.Infrastructure.Exceptions;
using System.Text.Json;

namespace remote_pokedex.Infrastructure.Services
{
    public abstract class BaseClient(string baseUrl) : IDisposable
    {
        private readonly HttpClient _httpClient = new();
        private readonly string _baseUrl = baseUrl;

        public async Task<T> GetAsync<T>(string route) where T : class
        {
            string url = _baseUrl + "/" + route;
            HttpResponseMessage? response;

            try
            {
                response = await _httpClient.GetAsync(url);
            }
            catch (Exception ex)
            {
                throw new HttpClientException($"Missing response from calling: {url}", HttpResponseErrorType.FAILED, ex);
            }

            return await HandleResponse<T>(response);
        }

        public async Task<T> PostAsync<T>(
            string route, 
            IEnumerable<(string, string)>? routeParams = null, 
            object? body = null
        ) where T : class 
        {
            string url = _baseUrl + "/" + route;
            HttpResponseMessage? response;

            if (routeParams is not null && routeParams.Any())
            {
                url += "?";
                url += string.Join('&', routeParams
                    .Select(p => p.Item1 + "=" + Uri.EscapeDataString(p.Item2.Replace("\n", " "))));
            }

            try
            {
                response = await _httpClient.PostAsJsonAsync(url, JsonSerializer.Serialize(body));
            }
            catch (Exception ex)
            {
                throw new HttpClientException($"Missing response from calling: {url}", HttpResponseErrorType.FAILED, ex);
            }

            return await HandleResponse<T>(response);

        }

        private async Task<T> HandleResponse<T>(HttpResponseMessage response) where T : class
        {
            if (response is null || !response.IsSuccessStatusCode)
                throw new HttpClientException($"Response was empty", HttpResponseErrorType.EMPTY);

            if (!response.IsSuccessStatusCode)
                throw new HttpClientException($"Response was not successful: {response?.StatusCode}", HttpResponseErrorType.FAILED);

            var stream = await response.Content.ReadAsStreamAsync();
            
            if (stream.Length <= 0)
                throw new HttpClientException($"Response body was empty and not deserializable into {nameof(T)}.", HttpResponseErrorType.EMPTY);

            T? resource = null;
            try
            {
                resource = await JsonSerializer.DeserializeAsync<T>(stream);
            }
            catch (Exception ex) when (ex is JsonException or ArgumentNullException)
            {
                throw new HttpClientException($"It was not possible to deserialize response to {nameof(T)}. Response: {response.Content?.ToString() ?? ""}", HttpResponseErrorType.FAILED, ex);
            }

            return resource ?? throw new HttpClientException("Response was empty", HttpResponseErrorType.EMPTY);
        }

        public void Dispose() => _httpClient.Dispose();
    }
}
