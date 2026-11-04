using PlataformaNexbank.Domain.ValueObjects;

namespace PlataformaNexbank.Domain.Events;

public sealed record ContaCriada(ContaId ContaId) : DomainEvent;