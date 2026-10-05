namespace PlataformaNexbank.Domain.ValueObjects;

public readonly record struct ContaId
{
    public Guid Valor { get; }

    // Construtor privado: a criação passa pelas factories, que garantem a validação
    private ContaId(Guid valor) => Valor = valor;

    public static ContaId Novo() => new(Guid.NewGuid());

    public static ContaId De(Guid valor)
    {
        if (valor == Guid.Empty)
            throw new ArgumentException("O identificador da conta não pode ser vazio.", nameof(valor));

        return new ContaId(valor);
    }

    public override string ToString() => Valor.ToString();
}