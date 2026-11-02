namespace PlataformaNexbank.Domain;

// Raiz de agregado: único ponto de entrada para alterar o agregado.
// Por enquanto só marca a fronteira; no dia 11 passa a acumular Domain Events.
public abstract class AggregateRoot<TId> : Entity<TId> where TId : notnull
{
    protected AggregateRoot(TId id) : base(id) { }

    // Usado apenas pelo EF Core para materialização
    protected AggregateRoot() { }
}