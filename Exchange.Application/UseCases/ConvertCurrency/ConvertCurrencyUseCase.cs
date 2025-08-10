using Exchange.Application.Interfaces;
using Exchange.Domain.Entities;
using Exchange.Domain.Interfaces;

namespace Exchange.Application.UseCases.ConvertCurrency
{
    public class ConvertCurrencyUseCase : IConvertCurrencyUseCase
    {
        private readonly IExchangeRateProvider _rateProvider;
        private readonly IConversionRepository _repository;

        public ConvertCurrencyUseCase(IExchangeRateProvider rateProvider, IConversionRepository repository)
        {
            _rateProvider = rateProvider;
            _repository = repository;
        }

        public async Task<ConvertCurrencyResponse> ExecuteAsync(ConvertCurrencyRequest request)
        {
            if (request.Amount <= 0)
                throw new ArgumentException("O valor deve ser maior que zero.");

            var rate = await _rateProvider.GetExchangeRateAsync(request.FromCurrency, request.ToCurrency);
            var convertedAmount = request.Amount * rate;

            var record = new ConversionRecord(
                request.FromCurrency,
                request.ToCurrency,
                request.Amount,
                convertedAmount,
                rate
            );

            await _repository.SaveAsync(record);

            return new ConvertCurrencyResponse(
                request.Amount,
                request.FromCurrency,
                convertedAmount,
                request.ToCurrency,
                rate
            );
        }
    }
}