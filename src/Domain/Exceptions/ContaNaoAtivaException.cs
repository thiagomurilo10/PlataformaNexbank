using PlataformaNexbank.Domain.Enums;

namespace PlataformaNexbank.Domain.Exceptions;

// Lançada quando há tentativa de movimentar uma conta que não está Ativa.
public class ContaNaoAtivaException : DomainException
{
    public StatusConta Status { get; }

    public ContaNaoAtivaException(StatusConta status)
        : base($"Operação não permitida: a conta está {status}.")
    {
        Status = status;
    }
}