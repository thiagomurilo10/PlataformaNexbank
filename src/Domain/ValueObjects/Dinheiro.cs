namespace NexBank.Domain.ValueObjects;

// Value Object que representa uma quantia monetária, é imutável e com igualdade por valor
public record Dinheiro
{
    public decimal Valor { get; }
    public string Moeda { get; }

    public Dinheiro(decimal valor, string moeda = "BRL")
    {
        // Regra de negócio: não existe dinheiro negativo neste domínio.
        if (valor < 0)
            throw new ArgumentException("Valor não pode ser negativo.", nameof(valor));

        if (string.IsNullOrWhiteSpace(moeda))
            throw new ArgumentException("Moeda é obrigatória.", nameof(moeda));

        Valor = valor;
        Moeda = moeda.ToUpperInvariant();
    }

    public static Dinheiro operator +(Dinheiro a, Dinheiro b)
    {
        ValidarMesmaMoeda(a, b);
        return new Dinheiro(a.Valor + b.Valor, a.Moeda);
    }

    public static Dinheiro operator -(Dinheiro a, Dinheiro b)
    {
        ValidarMesmaMoeda(a, b);
        return new Dinheiro(a.Valor - b.Valor, a.Moeda);
    }

    public static bool operator >(Dinheiro a, Dinheiro b)
    {
        ValidarMesmaMoeda(a, b);
        return a.Valor > b.Valor;
    }

    public static bool operator <(Dinheiro a, Dinheiro b)
    {
        ValidarMesmaMoeda(a, b);
        return a.Valor < b.Valor;
    }

    public static bool operator >=(Dinheiro a, Dinheiro b)
    {
        ValidarMesmaMoeda(a, b);
        return a.Valor >= b.Valor;
    }

    public static bool operator <=(Dinheiro a, Dinheiro b)
    {
        ValidarMesmaMoeda(a, b);
        return a.Valor <= b.Valor;
    }

    // Impede operações entre moedas diferentes (ex: BRL + USD não faz sentido sem conversão).
    private static void ValidarMesmaMoeda(Dinheiro a, Dinheiro b)
    {
        if (a.Moeda != b.Moeda)
            throw new InvalidOperationException($"Não é possível operar {a.Moeda} com {b.Moeda}.");
    }

    public override string ToString() => $"{Moeda} {Valor:N2}";
}