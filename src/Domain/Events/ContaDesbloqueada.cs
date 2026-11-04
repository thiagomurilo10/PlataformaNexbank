using PlataformaNexbank.Domain.ValueObjects;

namespace PlataformaNexbank.Domain.Events;

public sealed record ContaDesbloqueada(ContaId ContaId) : DomainEvent;