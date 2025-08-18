using Exchange.API.Middleware;
using Exchange.Application.Interfaces;
using Exchange.Application.UseCases.ConvertCurrency;
using Exchange.Domain.Interfaces;
using Exchange.Infrastructure.Repositories;
using Exchange.Infrastructure.Services.Bacen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IExchangeRateProvider, ExchangeRateProvider>();
builder.Services.AddScoped<IConversionRepository, ConversionRepository>();
builder.Services.AddScoped<IConvertCurrencyUseCase, ConvertCurrencyUseCase>();

// Add provider http client
builder.Services.AddHttpClient<IExchangeRateProvider, ExchangeRateProvider>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// register middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
