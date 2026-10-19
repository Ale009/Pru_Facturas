using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Facturas.Exceptions;

/// <summary>Traduce excepciones a respuestas ProblemDetails en un único lugar.</summary>
public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) => _logger = logger;

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is ReglaNegocioException)
        {
            var problema = new ProblemDetails
            {
                Status = StatusCodes.Status422UnprocessableEntity,
                Title = "Regla de negocio no cumplida",
                Detail = exception.Message
            };

            httpContext.Response.StatusCode = problema.Status.Value;
            await httpContext.Response.WriteAsJsonAsync(problema, cancellationToken);
            return true;
        }

        // Cualquier otro error: se registra y se deja al manejador por defecto (500 genérico).
        _logger.LogError(exception, "Error no controlado");
        return false;
    }
}