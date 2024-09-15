using remote_pokedex.Infrastructure.Endpoints;
using remote_pokedex.Pokemons.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddScoped<IPokeAPIRepository, PokeAPIRepository>();
builder.Services.AddScoped<IFunTranslationsService, FunTranslationsService>();

// Add services to the container.
builder.Services.AddEndpoints();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapEndpoints();

app.Run();
