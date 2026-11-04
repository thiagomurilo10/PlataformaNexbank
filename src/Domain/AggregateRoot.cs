using PlataformaNexbank.Domain.Events;

namespace PlataformaNexbank.Domain;

// Raiz de agregado: único ponto de entrada para alterar o agregado.
// Por enquanto só marca a fronteira; no dia 11 passa a acumular Domain Events.
public abstract class AggregateRoot<TId> : Entity<TId> where TId : notnull
{
    protected AggregateRoot(TId id) : base(id) { }

    // Usado apenas pelo EF Core para materialização
    protected AggregateRoot() { }

    private readonly List<IDomainEvent> _domainEvents = new();

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AdicionarEvento(IDomainEvent evento)
    {
        ArgumentNullException.ThrowIfNull(evento);
        _domainEvents.Add(evento);
    }

    public void LimparEventos() => _domainEvents.Clear();
}