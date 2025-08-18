using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Domain.Entities
{
    public class ExchangeRate
    {
        public string Currency { get; private set; }
        public decimal BuyRate { get; private set; }
        public decimal SellRate { get; private set; }
        public string QuotationDate { get; private set; }

        public ExchangeRate(string currency, decimal buyRate, decimal sellRate, string quotationDate)
        {
            Currency = currency;
            BuyRate = buyRate;
            SellRate = sellRate;
            QuotationDate = quotationDate;
        }
    }
}