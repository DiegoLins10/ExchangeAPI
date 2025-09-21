using Exchange.Domain.Enums;

namespace Exchange.Application.Dtos.Requests
{
    public record ConvertCurrencyRequest(string ToCurrency, decimal AmountBRL, DateOnly DateQuotation, ExchangeQuotationEnum ExchangeType );
}