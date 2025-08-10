using Exchange.Domain.Interfaces;

namespace Exchange.Infrastructure.Services
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private static readonly Random _random = new();

        public Task<decimal> GetExchangeRateAsync(string fromCurrency, string toCurrency)
        {
            // Simula taxa de câmbio aleatória (1.0 a 6.0)
            decimal rate = Math.Round((decimal)(_random.NextDouble() * (6 - 1) + 1), 2);
            return Task.FromResult(rate);
        }
    }
}
