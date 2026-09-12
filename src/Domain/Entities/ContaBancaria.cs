namespace PlataformaNexbank.Domain.Entities;

public class ContaBancaria
{
    public Guid Id { get; private set; }
    public string Titular { get; private set; }
    public decimal Saldo { get; private set; }

    public ContaBancaria(string titular)
    {
        if (string.IsNullOrWhiteSpace(titular))
        {
            throw new ArgumentException("Titular não pode ser vazio.", nameof(titular));
        }

        Id = Guid.NewGuid();
        Titular = titular;
        Saldo = 0;
    }
}
