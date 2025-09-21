namespace Exchange.Application.Dtos.Responses
{
    public record ConvertCurrencyResponse(decimal OriginalAmount, string FromCurrency, decimal ConvertedAmount, string ToCurrency, decimal ExchangeRate);

}
