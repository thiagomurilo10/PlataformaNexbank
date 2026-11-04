using PlataformaNexbank.Domain.ValueObjects;

namespace PlataformaNexbank.Domain.Events;

public sealed record ContaEncerrada(ContaId ContaId) : DomainEvent;