using PlataformaNexbank.Domain.Enums;

namespace PlataformaNexbank.Domain.Entities;

// Representa uma movimentação financeira (depósito ou saque) já ocorrida.
// É imutável: uma vez criada, seus dados não podem ser alterados, pois representa um histórico
public class Transacao
{
    public Guid Id { get; private set; }         // Identificador único da transação
    public TipoTransacao Tipo { get; private set; }     // Se foi depósito ou saque
    public decimal Valor { get; private set; }      // Valor movimentado (sempre positivo)
    public DateTime DataHora { get; private set; }      // Registra o momento q foi feito a transacao

    public Transacao(TipoTransacao tipo, decimal valor)
    {
        Id = Guid.NewGuid();
        Tipo = tipo;
        Valor = valor;
        DataHora = DateTime.UtcNow;
    }
}