using Exchange.Application.Dtos.Requests;
using Exchange.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Exchange.API.Controllers
{
    [ApiController]
    [Route("api/currency")]
    public class CurrencyController : ControllerBase
    {
        private readonly IConvertCurrencyUseCase _convertCurrencyUseCase;
        private readonly IGetConversionHistoryUseCase _getConversionHistoryUseCase;

        public CurrencyController(IConvertCurrencyUseCase convertCurrencyUseCase, IGetConversionHistoryUseCase getConversionHistoryUseCase)
        {
            _convertCurrencyUseCase = convertCurrencyUseCase;
            _getConversionHistoryUseCase = getConversionHistoryUseCase;
        }

        [HttpPost("convert")]
        public async Task<IActionResult> Convert([FromBody] ConvertCurrencyRequest request)
        {
            var result = await _convertCurrencyUseCase.ExecuteAsync(request);
            return Ok(result);
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var history = await _getConversionHistoryUseCase.ExecuteAsync();
            return Ok(history);
        }
    }
}
