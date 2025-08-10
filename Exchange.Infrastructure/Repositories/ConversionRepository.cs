using Exchange.Domain.Entities;
using Exchange.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Infrastructure.Repositories
{
    public class ConversionRepository : IConversionRepository
    {
        private static readonly List<ConversionRecord> _data = new();

        public Task SaveAsync(ConversionRecord record)
        {
            _data.Add(record);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<ConversionRecord>> GetHistoryAsync()
        {
            return Task.FromResult(_data.AsEnumerable());
        }
    }
}
