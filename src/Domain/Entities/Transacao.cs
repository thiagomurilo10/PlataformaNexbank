using NexBank.Domain.ValueObjects;
using PlataformaNexbank.Domain.Enums;

namespace PlataformaNexbank.Domain.Entities;

// Representa uma movimentação financeira (depósito ou saque) já ocorrida.
// É imutável: uma vez criada, seus dados não podem ser alterados, pois representa um histórico
public class Transacao
{
    public TipoTransacao Tipo { get; }
    public Dinheiro Valor { get; }
    public DateTime Data { get; }

    public Transacao(TipoTransacao tipo, Dinheiro valor)
    {
        Tipo = tipo;
        Valor = valor;
        Data = DateTime.UtcNow;
    }
}