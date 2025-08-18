using Exchange.Domain.Entities;
using Exchange.Domain.Interfaces;
using Exchange.Infrastructure.Services.Bacen.Responses;
using System.Net.Http.Json;

namespace Exchange.Infrastructure.Services.Bacen
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly HttpClient _httpClient;

        public ExchangeRateProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ExchangeRate> GetExchangeRateAsync(string currency, DateOnly date)
        {
            var url = $"https://olinda.bcb.gov.br/olinda/servico/PTAX/versao/v1/odata/" +
                      $"CotacaoMoedaDia(moeda=@moeda,dataCotacao=@dataCotacao)" +
                      $"?@moeda='{currency}'&@dataCotacao='{date:MM-dd-yyyy}'&$top=1&$format=json";

            var response = await _httpClient.GetFromJsonAsync<CurrencyBacenResponse>(url);

            CurrencyItemBacen item = response?.Value?.FirstOrDefault()
                       ?? throw new InvalidOperationException("No exchange rate found.");
            
            return new ExchangeRate(
                currency,
                item.CotacaoCompra,
                item.CotacaoVenda,
                item.DataHoraCotacao
            );
        }
    }
}
