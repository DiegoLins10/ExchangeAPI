﻿namespace Exchange.Domain.Entities
{
    public class ConversionRecord
    {
        public Guid Id { get; private set; }
        public string FromCurrency { get; private set; }
        public string ToCurrency { get; private set; }
        public decimal OriginalAmount { get; private set; }
        public decimal ConvertedAmount { get; private set; }
        public decimal ExchangeRate { get; private set; }
        public DateTime ConversionDate { get; private set; }

        public ConversionRecord(string fromCurrency, string toCurrency, decimal originalAmount, decimal convertedAmount, decimal exchangeRate)
        {
            Id = Guid.NewGuid();
            FromCurrency = fromCurrency;
            ToCurrency = toCurrency;
            OriginalAmount = originalAmount;
            ConvertedAmount = convertedAmount;
            ExchangeRate = exchangeRate;
            ConversionDate = DateTime.UtcNow;
        }
    }
}
