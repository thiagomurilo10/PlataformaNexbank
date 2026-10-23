namespace PlataformaNexbank.Domain.Exceptions;

// Invariante: conta só pode ser encerrada com saldo zero.
public class ContaComSaldoException : DomainException
{
    public decimal Saldo { get; }

    public ContaComSaldoException(decimal saldo)
        : base($"A conta não pode ser encerrada com saldo {saldo}.")
    {
        Saldo = saldo;
    }
}