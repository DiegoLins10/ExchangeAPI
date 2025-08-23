using Exchange.Domain.Entities;
using Exchange.Domain.Interfaces;
using Exchange.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;

namespace Exchange.Infrastructure.Repositories
{
    public class ConversionRepository : IConversionRepository
    {
        private readonly ExchangeDbContext _context;

        public ConversionRepository(ExchangeDbContext context)
        {
            _context = context;
        }

        public async Task SaveAsync(ConversionRecord record)
        {
            await _context.ConversionRecords.AddAsync(record);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ConversionRecord>> GetHistoryAsync()
        {
            return await _context.ConversionRecords
                                 .OrderByDescending(c => c.ConversionDate) // exemplo de ordenação
                                 .ToListAsync();
        }
    }
}
