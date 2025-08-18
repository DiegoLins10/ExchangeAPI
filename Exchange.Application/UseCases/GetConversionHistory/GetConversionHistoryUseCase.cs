using Exchange.Application.Interfaces;
using Exchange.Domain.Entities;
using Exchange.Domain.Interfaces;

namespace Exchange.Application.UseCases.GetConversionHistory
{
    public class GetConversionHistoryUseCase : IGetConversionHistoryUseCase
    {
        private readonly IConversionRepository _repository;


        public GetConversionHistoryUseCase(IConversionRepository repository)
        {
            _repository = repository;

        }

        public async Task<IEnumerable<ConversionRecord>> ExecuteAsync()
        {
            var history = await _repository.GetHistoryAsync();

            if (!history.Any())
            {
                return new List<ConversionRecord>();
            }

            return history;
        }

    }
}
