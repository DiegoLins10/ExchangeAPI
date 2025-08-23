using Exchange.Application.Interfaces;
using Exchange.Domain.Entities;
using Exchange.Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Exchange.Application.UseCases.GetConversionHistory
{
    public class GetConversionHistoryUseCase : IGetConversionHistoryUseCase
    {
        private readonly IConversionRepository _repository;
        private readonly IMemoryCache _memoryCache;

        // chave única pro cache
        private const string CacheKey = "conversion_history";


        public GetConversionHistoryUseCase(IConversionRepository repository, IMemoryCache memoryCache)
        {
            _repository = repository;
            _memoryCache = memoryCache;

        }

        public async Task<IEnumerable<ConversionRecord>> ExecuteAsync()
        {

            // obs. Não utilizar directionary como cache, ele não é thread-safe
            // Dictionary → não é seguro pra concorrência (precisa de ConcurrentDictionary ou lock)
            // MemoryCache → já é seguro pra multithread.

            // tenta pegar do cache
            if (_memoryCache.TryGetValue(CacheKey, out IEnumerable<ConversionRecord> cachedHistory))
            {
                return cachedHistory;
            }

            // busca no repositório
            var history = await _repository.GetHistoryAsync();

            if (!history.Any())
            {
                return new List<ConversionRecord>();
            }

            // adiciona no cache com expiração de 5 minutos (ajuste conforme necessidade)
            _memoryCache.Set(CacheKey, history, TimeSpan.FromSeconds(60));

            return history;
        }

    }
}
