namespace PlataformaNexbank.Domain.ValueObjects;

public sealed record Cpf
{
    private const int Tamanho = 11;

    public string Numero { get; }

    // Máscara derivada do número; não é armazenada.
    public string Formatado => $"{Numero[..3]}.{Numero[3..6]}.{Numero[6..9]}-{Numero[9..]}";

    private Cpf(string numero) => Numero = numero;

    public static Cpf Criar(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("CPF é obrigatório.", nameof(valor));

        // Remove máscara e qualquer caractere não numérico.
        var numero = new string(valor.Where(char.IsAsciiDigit).ToArray());

        if (numero.Length != Tamanho)
            throw new ArgumentException("CPF deve conter 11 dígitos.", nameof(valor));

        // Sequências repetidas passam no cálculo de dígitos, mas são inválidas.
        if (numero.Distinct().Count() == 1)
            throw new ArgumentException("CPF inválido.", nameof(valor));

        if (!DigitosVerificadoresValidos(numero))
            throw new ArgumentException("CPF inválido.", nameof(valor));

        return new Cpf(numero);
    }

    public override string ToString() => Numero;

    private static bool DigitosVerificadoresValidos(string numero)
    {
        var primeiro = CalcularDigito(numero, 9);
        var segundo = CalcularDigito(numero, 10);

        return numero[9] - '0' == primeiro && numero[10] - '0' == segundo;
    }

    // Pesos decrescentes: (quantidade + 1) até 2. Resto < 2 resulta em dígito 0.
    private static int CalcularDigito(string numero, int quantidade)
    {
        var soma = 0;
        for (var i = 0; i < quantidade; i++)
            soma += (numero[i] - '0') * (quantidade + 1 - i);

        var resto = soma % 11;
        return resto < 2 ? 0 : 11 - resto;
    }
}