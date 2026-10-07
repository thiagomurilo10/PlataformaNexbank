namespace PlataformaNexbank.Domain;

public abstract class Entity<TId> : IEquatable<Entity<TId>> where TId : notnull
{
    public TId Id { get; protected set; }

    protected Entity(TId id) => Id = id;

    // Usado apenas pelo EF Core para materialização
    protected Entity() => Id = default!;

    private bool IdEhPadrao() => EqualityComparer<TId>.Default.Equals(Id, default!);

    public bool Equals(Entity<TId>? outra)
    {
        if (outra is null) return false;
        if (ReferenceEquals(this, outra)) return true;
        if (GetType() != outra.GetType()) return false;
        // Entidades sem identidade atribuída só são iguais por referência
        if (IdEhPadrao() || outra.IdEhPadrao()) return false;

        return EqualityComparer<TId>.Default.Equals(Id, outra.Id);
    }

    public override bool Equals(object? obj) => Equals(obj as Entity<TId>);

    public override int GetHashCode() => HashCode.Combine(GetType(), Id);

    public static bool operator ==(Entity<TId>? esquerda, Entity<TId>? direita) =>
        esquerda is null ? direita is null : esquerda.Equals(direita);

    public static bool operator !=(Entity<TId>? esquerda, Entity<TId>? direita) =>
        !(esquerda == direita);
}