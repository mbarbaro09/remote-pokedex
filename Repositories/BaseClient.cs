using remote_pokedex.Infrastructure.Exceptions;
using System.Text.Json;

namespace remote_pokedex.Repositories
{
    public abstract class BaseClient(string baseUrl) : IDisposable
    {
        private HttpClient _httpClient = new HttpClient();
        private string _baseUrl = baseUrl;

        public async Task<T> GetAsync<T>(string resourceUrl) where T : class 
        {
            string url = _baseUrl + "/" + resourceUrl;
            HttpResponseMessage? response;

            try
            {
                response = await _httpClient.GetAsync(url);
            } 
            catch (Exception ex) 
            {
                throw new HttpClientException($"Missing response from calling: {url}", ex);
            }

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
