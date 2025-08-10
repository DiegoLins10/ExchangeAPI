namespace Exchange.Domain.Interfaces
{
    public interface IExchangeRateProvider
    {
        Task<decimal> GetExchangeRateAsync(string fromCurrency, string toCurrency);
    }
}
