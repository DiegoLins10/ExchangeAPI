namespace Exchange.Application.UseCases.ConvertCurrency
{
    public record ConvertCurrencyRequest(string FromCurrency, string ToCurrency, decimal Amount);
}