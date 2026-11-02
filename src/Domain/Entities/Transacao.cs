using PlataformaNexbank.Domain.Enums;
using PlataformaNexbank.Domain.ValueObjects;

namespace PlataformaNexbank.Domain.Entities;

// Representa uma movimentação financeira (depósito ou saque) já ocorrida.
// Entidade interna do agregado ContaBancaria: imutável e criada apenas pela raiz.
public class Transacao
{
    public TipoTransacao Tipo { get; }
    public Dinheiro Valor { get; }
    public DateTime Data { get; }

    // Construtor privado exclusivo para materialização do EF Core.
    private Transacao()
    {
        Valor = null!;
    }

    // internal: só o agregado ContaBancaria (mesmo assembly) cria transações,
    // garantindo que toda transação corresponda a uma alteração de saldo.
    internal Transacao(TipoTransacao tipo, Dinheiro valor)
    {
        Tipo = tipo;
        Valor = valor;
        Data = DateTime.UtcNow;
    }
}