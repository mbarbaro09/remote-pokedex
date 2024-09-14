using remote_pokedex.Infrastructure.Services;

namespace remote_pokedex.Pokemons;

public interface IFunTranslationsService
{
    Task<string> GetShakespeareTranslation(string text);
    Task<string> GetYodaTranslation(string text);
}

public class FunTranslationsService(string baseUrl) : BaseClient(baseUrl), IFunTranslationsService
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
