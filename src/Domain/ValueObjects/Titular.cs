namespace PlataformaNexbank.Domain.ValueObjects;

public sealed record Titular
{
    private const int TamanhoMinimoNome = 3;

    public string Nome { get; }
    public Cpf Cpf { get; }

    private Titular(string nome, Cpf cpf)
    {
        Nome = nome;
        Cpf = cpf;
    }

    // Construtor privado exclusivo para materialização do EF Core.
    private Titular()
    {
        Nome = null!;
        Cpf = null!;
    }

    public static Titular Criar(string? nome, Cpf cpf)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do titular é obrigatório.", nameof(nome));

        // Validação após o trim para que espaços não contem como caracteres.
        var nomeNormalizado = nome.Trim();

        if (nomeNormalizado.Length < TamanhoMinimoNome)
            throw new ArgumentException(
                $"Nome do titular deve ter no mínimo {TamanhoMinimoNome} caracteres.", nameof(nome));

        ArgumentNullException.ThrowIfNull(cpf);

        return new Titular(nomeNormalizado, cpf);
    }
}