using remote_pokedex.Infrastructure.Endpoints;
using remote_pokedex.Pokemons.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<IPokeAPIRepository, PokeAPIRepository>((provider) => new PokeAPIRepository(builder.Configuration.GetSection("ServicesURL").GetValue<string>("PokeAPI")));
builder.Services.AddScoped<IFunTranslationsService, FunTranslationsService>((provider) => new FunTranslationsService(builder.Configuration.GetSection("ServicesURL").GetValue<string>("FunTranslations")));

// Add services to the container.
builder.Services.AddEndpoints();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapEndpoints();

app.Run();
