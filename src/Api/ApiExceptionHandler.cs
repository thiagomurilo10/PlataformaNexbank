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
        // Mapeamento exceção → status HTTP. A ordem importa: tipos específicos antes dos genéricos.
        // CpfJaCadastradoException → 409 (conflito com recurso existente). Vem antes de DomainException, da qual herda.
        // DomainException → 422: requisição bem formada, mas viola regra de negócio (saldo, status da conta...).
        // ArgumentException → 400: dado de entrada inválido.
        // 404 não passa por aqui: os endpoints retornam NotFound quando o use case devolve null.
        // Qualquer outra → 500, sem vazar detalhes internos.
        var (status, title) = exception switch
        {
            CpfJaCadastradoException => (StatusCodes.Status409Conflict, exception.Message),
            DomainException => (StatusCodes.Status422UnprocessableEntity, exception.Message),
            ArgumentException => (StatusCodes.Status400BadRequest, exception.Message),
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