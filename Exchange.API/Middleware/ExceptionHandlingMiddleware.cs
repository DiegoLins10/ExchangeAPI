using System.Text.Json;

namespace Exchange.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "ArgumentException capturada");

                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                httpContext.Response.ContentType = "application/json";

                var result = JsonSerializer.Serialize(new { error = ex.Message });
                await httpContext.Response.WriteAsync(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro não tratado");

                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                httpContext.Response.ContentType = "application/json";

                var result = JsonSerializer.Serialize(new { error = "Ocorreu um erro inesperado." });
                await httpContext.Response.WriteAsync(result);
            }
        }
    }

}
