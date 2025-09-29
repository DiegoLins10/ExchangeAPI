using Exchange.API.Middleware;
using Exchange.Application.Interfaces;
using Exchange.Application.UseCases.AuthenticateClient;
using Exchange.Application.UseCases.ConvertCurrency;
using Exchange.Application.UseCases.GetConversionHistory;
using Exchange.Domain.Entities;
using Exchange.Domain.Interfaces;
using Exchange.Infrastructure.Persistences;
using Exchange.Infrastructure.Repositories;
using Exchange.Infrastructure.Services.Authentication;
using Exchange.Infrastructure.Services.Bacen;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// add database
builder.Services.AddDbContext<ExchangeDbContext>(options =>
    options.UseInMemoryDatabase("ExchangeDb"));

// Add services to the container.
builder.Services.AddScoped<IExchangeRateProvider, ExchangeRateProvider>();
builder.Services.AddScoped<IConvertCurrencyUseCase, ConvertCurrencyUseCase>();
builder.Services.AddScoped<IGetConversionHistoryUseCase, GetConversionHistoryUseCase>();
builder.Services.AddScoped<IAuthenticateClientUseCase, AuthenticateClientUseCase>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

// Add repositories
builder.Services.AddScoped<IConversionRepository, ConversionRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();

// Add provider http client
builder.Services.AddHttpClient<IExchangeRateProvider, ExchangeRateProvider>();

// add memory cache
builder.Services.AddMemoryCache();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks();

// add authentication
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Inicializar clientes no banco em memória usando o DI (Injeção de Dependência)
using (var scope = app.Services.CreateScope())
{
    var scopedServices = scope.ServiceProvider;
    var repository = scopedServices.GetRequiredService<IClientRepository>();

    await repository.AddClientAsync(new Client { ClientId = "3f29b6e7-1c4b-4f9a-b8b4-2f5e2f4d5c6a", Secret = "f8d9a7b6-2c3e-4f7a-8b1d-3e2f4a5b6c7d" });
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// register middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

// Mapeia endpoint /health
app.MapHealthChecks("/health");

app.UseAuthorization();

app.MapControllers();

app.Run();