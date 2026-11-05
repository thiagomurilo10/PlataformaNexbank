using PlataformaNexbank.Domain.ValueObjects;

namespace PlataformaNexbank.Domain.Events;

public sealed record DepositoRealizado(ContaId ContaId, Dinheiro Valor, Dinheiro SaldoApos) : DomainEvent;