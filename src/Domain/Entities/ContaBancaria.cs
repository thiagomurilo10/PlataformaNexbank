using PlataformaNexbank.Domain.Enums;
using PlataformaNexbank.Domain.Exceptions;
using PlataformaNexbank.Domain.ValueObjects;

namespace PlataformaNexbank.Domain.Entities;

public class ContaBancaria
{
    // Lista interna e mutável — só a própria entidade pode adicionar transações.
    private readonly List<Transacao> _transacoes = new();

    public Guid Id { get; private set; }

    // Value Object: nome e CPF já chegam validados, a conta não repete essas regras.
    public Titular Titular { get; private set; }
    public Dinheiro Saldo { get; private set; }

    // Quem consome a conta pode LER o histórico, mas não alterar a lista diretamente.
    public IReadOnlyList<Transacao> Transacoes => _transacoes.AsReadOnly();

    // Construtor privado exclusivo para materialização do EF Core.
    private ContaBancaria()
    {
        Titular = null!;
        Saldo = null!;
    }

    public ContaBancaria(Titular titular)
    {
        // Titular sempre é válido por construção; só é preciso garantir que existe.
        ArgumentNullException.ThrowIfNull(titular);

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