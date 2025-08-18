using Exchange.Domain.Enums;

namespace Exchange.Application.UseCases.ConvertCurrency
{
    public record ConvertCurrencyRequest(string ToCurrency, decimal AmountBRL, DateOnly DateQuotation, ExchangeQuotationEnum ExchangeType );
}