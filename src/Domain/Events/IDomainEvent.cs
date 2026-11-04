namespace PlataformaNexbank.Domain.Events;

public interface IDomainEvent
{
    Guid EventoId { get; }
    DateTime OcorridoEm { get; }
}