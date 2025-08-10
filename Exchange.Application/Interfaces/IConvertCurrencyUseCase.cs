using Exchange.Application.UseCases.ConvertCurrency;

namespace Exchange.Application.Interfaces
{
    public interface IConvertCurrencyUseCase
    {
        Task<ConvertCurrencyResponse> ExecuteAsync(ConvertCurrencyRequest request);

    }
}