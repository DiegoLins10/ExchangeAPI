using Exchange.Application.UseCases.ConvertCurrency;
using Exchange.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Application.Interfaces
{
    public interface IGetConversionHistoryUseCase
    {
        Task<IEnumerable<ConversionRecord>> ExecuteAsync();

    }
}
