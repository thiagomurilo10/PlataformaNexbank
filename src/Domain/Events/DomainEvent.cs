namespace PlataformaNexbank.Domain.Events;

public abstract record DomainEvent : IDomainEvent
{
    public Guid EventoId { get; } = Guid.NewGuid();

    // Débito conhecido: substituir por TimeProvider na semana 4
    public DateTime OcorridoEm { get; } = DateTime.UtcNow;
}