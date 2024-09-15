using remote_pokedex.Infrastructure.Services;
using remote_pokedex.Pokemons.Services.DTOs;

namespace remote_pokedex.Pokemons.Services;

public interface IFunTranslationsService
{
    Task<string> GetShakespeareTranslation(string text);
    Task<string> GetYodaTranslation(string text);
}

/// <summary>
/// Client that calls the translation service via REST API
/// </summary>
public class FunTranslationsService(IConfiguration configuration, ILogger<FunTranslationsService> logger) 
    : BaseClient(configuration.GetSection("ServicesURL").GetValue<string>("FunTranslations"), logger), IFunTranslationsService
{
    public async Task<string> GetShakespeareTranslation(string text)
    {
        Translation translation = await PostAsync<Translation>($"shakespeare.json", routeParams: [("text", text)]);
        return translation.contents.translated;
    }

    public async Task<string> GetYodaTranslation(string text)
    {
        Translation translation = await PostAsync<Translation>($"yoda.json", routeParams: [("text", text)]);
        return translation.contents.translated;
    }
}
