using PlataformaNexbank.Domain.Enums;

namespace PlataformaNexbank.Domain.Exceptions;

// Lançada quando a máquina de estados da conta não permite a transição pedida.
public class TransicaoStatusInvalidaException : DomainException
{
    public StatusConta Atual { get; }
    public StatusConta Destino { get; }

    public TransicaoStatusInvalidaException(StatusConta atual, StatusConta destino)
        : base($"Não é possível mudar a conta de {atual} para {destino}.")
    {
        Atual = atual;
        Destino = destino;
    }
}