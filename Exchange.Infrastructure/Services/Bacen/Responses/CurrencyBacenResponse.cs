namespace Exchange.Infrastructure.Services.Bacen.Responses
{
    public class CurrencyBacenResponse
    {
        public List<CurrencyItemBacen> Value { get; set; } = new();
    }

    public class CurrencyItemBacen
    {
        public decimal ParidadeCompra { get; set; }
        public decimal ParidadeVenda { get; set; }
        public decimal CotacaoCompra { get; set; }
        public decimal CotacaoVenda { get; set; }
        public DateTime DataHoraCotacao { get; set; }
        public string TipoBoletim { get; set; } = string.Empty;
    }
}
