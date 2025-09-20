using Exchange.Application.Dtos.Requests;
using Exchange.Application.Dtos.Responses;

namespace Exchange.Application.Interfaces
{
    public interface IConvertCurrencyUseCase
    {
        Task<ConvertCurrencyResponse> ExecuteAsync(ConvertCurrencyRequest request);

    }
}