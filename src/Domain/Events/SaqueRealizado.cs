using PlataformaNexbank.Domain.ValueObjects;

namespace PlataformaNexbank.Domain.Events;

public sealed record SaqueRealizado(ContaId ContaId, Dinheiro Valor, Dinheiro SaldoApos) : DomainEvent;