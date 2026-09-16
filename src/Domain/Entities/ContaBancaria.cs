using NexBank.Domain.ValueObjects;
using PlataformaNexbank.Domain.Enums;
using PlataformaNexbank.Domain.Exceptions;

namespace PlataformaNexbank.Domain.Entities;

public class ContaBancaria
{
    // Lista interna e mutável — só a própria entidade pode adicionar transações.
    private readonly List<Transacao> _transacoes = new();

    public Guid Id { get; private set; }
    public string Titular { get; private set; }
    public Dinheiro Saldo { get; private set; }

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
        Saldo = new Dinheiro(0);
    }

    public void Depositar(decimal valor)
{
    var dinheiro = new Dinheiro(valor);

    if (dinheiro.Valor <= 0)
        throw new ArgumentException("Valor de depósito deve ser maior que zero.");

    Saldo = Saldo + dinheiro; // usa o operador + do VO
    _transacoes.Add(new Transacao(TipoTransacao.Deposito, dinheiro));
}

public void Sacar(decimal valor)
{
    var dinheiro = new Dinheiro(valor);

    if (dinheiro.Valor <= 0)
        throw new ArgumentException("Valor de saque deve ser maior que zero.");

    if (Saldo < dinheiro)   // usa o operador < do VO
            throw new SaldoInsuficienteException(Saldo.Valor, dinheiro.Valor);

    Saldo = Saldo - dinheiro;
    _transacoes.Add(new Transacao(TipoTransacao.Saque, dinheiro));
}
}
