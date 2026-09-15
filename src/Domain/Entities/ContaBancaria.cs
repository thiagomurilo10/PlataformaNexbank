using PlataformaNexbank.Domain.Enums;
using PlataformaNexbank.Domain.Exceptions;

namespace PlataformaNexbank.Domain.Entities;

public class ContaBancaria
{
    // Lista interna e mutável — só a própria entidade pode adicionar transações.
    private readonly List<Transacao> _transacoes = new();

    public Guid Id { get; private set; }
    public string Titular { get; private set; }
    public decimal Saldo { get; private set; }

    // quem consome a conta pode LER o histórico, mas não pode adicionar/remover itens diretamente na lista.
    public IReadOnlyList<Transacao> Transacoes => _transacoes.AsReadOnly();

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

    public void Depositar(decimal valor)
    {
        if (valor <= 0)
            throw new ArgumentException("Valor do depósito deve ser positivo.", nameof(valor));

        Saldo += valor;
        // Registra o depósito no histórico da conta.
        _transacoes.Add(new Transacao(TipoTransacao.Deposito, valor));
    }

    public void Sacar(decimal valor)
    {
        if (valor <= 0)
            throw new ArgumentException("Valor do saque deve ser positivo.", nameof(valor));

        // não é permitido saldo negativo.
        if (valor > Saldo)
            throw new SaldoInsuficienteException(Saldo, valor);

        Saldo -= valor;

        // Registra o saque no histórico da conta.
        _transacoes.Add(new Transacao(TipoTransacao.Saque, valor));
    }
}
