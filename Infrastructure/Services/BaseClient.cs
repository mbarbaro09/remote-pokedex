using remote_pokedex.Infrastructure.Exceptions;
using System.Text.Json;

namespace remote_pokedex.Infrastructure.Services
{
    public abstract class BaseClient(string baseUrl) : IDisposable
    {
        private HttpClient _httpClient = new HttpClient();
        private string _baseUrl = baseUrl;

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
                throw new HttpClientException($"Missing response from calling: {url}", ex);
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
                throw new HttpClientException($"Missing response from calling: {url}", ex);
            }

            return await HandleResponse<T>(response);

        }

        private async Task<T> HandleResponse<T>(HttpResponseMessage response) where T : class
        {
            if (response is null || !response.IsSuccessStatusCode)
            {
                string content = JsonSerializer.Serialize(response?.Content);
                throw new HttpClientException($"Response was empty or not successful: {response?.StatusCode}, {content}");
            }

            T? resource = null;
            try
            {
                var stream = await response.Content?.ReadAsStreamAsync() ?? Stream.Null;
                resource = await JsonSerializer.DeserializeAsync<T>(stream);
            }
            catch (Exception ex)
            {
                throw new HttpClientException($"It was not possible to deserialize response to {nameof(T)}. Response: {response.Content?.ToString() ?? ""}", ex);
            }

            return resource ?? throw new HttpClientException("Response was empty");
        }

        public void Dispose() => _httpClient.Dispose();
    }
}
