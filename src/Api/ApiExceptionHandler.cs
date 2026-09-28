using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PlataformaNexbank.Domain.Exceptions;

namespace PlataformaNexbank.Api;

// Tratamento global de exceções da API.
// Centraliza a conversão de exceções de domínio em respostas HTTP padronizadas (ProblemDetails), eliminando a necessidade de try/catch em cada endpoint.
public class ApiExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // Mapeia cada tipo de exceção para o status HTTP correspondente. SaldoInsuficienteException e ArgumentException → 400.
        // KeyNotFoundException indica recurso não encontrado → 404. Qualquer outra exceção é tratada como erro inesperado → 500, sem vazar detalhes internos.
        var (status, title) = exception switch
        {
            SaldoInsuficienteException => (StatusCodes.Status400BadRequest, exception.Message),
            ArgumentException => (StatusCodes.Status400BadRequest, exception.Message),
            KeyNotFoundException => (StatusCodes.Status404NotFound, exception.Message),
            _ => (StatusCodes.Status500InternalServerError, "Erro interno no servidor.")
        };

        var problemDetails = new ProblemDetails
        {
            Status = status,
            Title = title
        };

        httpContext.Response.StatusCode = status;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}