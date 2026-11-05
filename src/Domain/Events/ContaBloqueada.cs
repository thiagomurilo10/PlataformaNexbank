using PlataformaNexbank.Domain.ValueObjects;

namespace PlataformaNexbank.Domain.Events;

public sealed record ContaBloqueada(ContaId ContaId) : DomainEvent;