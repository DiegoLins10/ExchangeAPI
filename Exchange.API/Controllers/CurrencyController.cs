using Exchange.Application.Interfaces;
using Exchange.Application.UseCases.ConvertCurrency;
using Exchange.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Exchange.API.Controllers
{
    [ApiController]
    [Route("api/currency")]
    public class CurrencyController : ControllerBase
    {
        private readonly IConvertCurrencyUseCase _convertCurrencyUseCase;
        private readonly IConversionRepository _repository;

        public CurrencyController(IConvertCurrencyUseCase convertCurrencyUseCase, IConversionRepository repository)
        {
            _convertCurrencyUseCase = convertCurrencyUseCase;
            _repository = repository;
        }

        [HttpPost("convert")]
        public async Task<IActionResult> Convert(ConvertCurrencyRequest request)
        {
            var result = await _convertCurrencyUseCase.ExecuteAsync(request);
            return Ok(result);
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var history = await _repository.GetHistoryAsync();
            return Ok(history);
        }
    }
}
