namespace Exchange.Application.UseCases.ConvertCurrency
{
    public record ConvertCurrencyResponse(decimal OriginalAmount, string FromCurrency, decimal ConvertedAmount, string ToCurrency, decimal ExchangeRate);

}
