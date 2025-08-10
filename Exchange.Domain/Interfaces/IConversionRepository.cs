using Exchange.Domain.Entities;

namespace Exchange.Domain.Interfaces
{
    public interface IConversionRepository
    {
        Task SaveAsync(ConversionRecord record);
        Task<IEnumerable<ConversionRecord>> GetHistoryAsync();
    }
}
