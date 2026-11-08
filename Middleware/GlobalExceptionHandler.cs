using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Data.SqlClient;

namespace RescatApp.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Error no controlado en {Ruta}", context.Request.Path);
            var (status, mensaje) = exception.GetBaseException() switch
            {
                SqlException { Number: 547 } => (StatusCodes.Status409Conflict, "La operación entra en conflicto con datos relacionados."),
                _ => (StatusCodes.Status500InternalServerError, "Ocurrió un error interno. Intente más tarde.")
            };

            context.Response.StatusCode = status;
            await context.Response.WriteAsJsonAsync(new { mensaje }, cancellationToken);
            return true;
        }

    }
}