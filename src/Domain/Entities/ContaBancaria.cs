using PlataformaNexbank.Domain.Enums;
using PlataformaNexbank.Domain.Exceptions;
using PlataformaNexbank.Domain.ValueObjects;

namespace PlataformaNexbank.Domain.Entities;

public class ContaBancaria : AggregateRoot<ContaId>
{
    // Lista interna e mutável — só a própria entidade pode adicionar transações.
    private readonly List<Transacao> _transacoes = new();

    // Value Object: nome e CPF já chegam validados, a conta não repete essas regras.
    public Titular Titular { get; private set; }
    public Dinheiro Saldo { get; private set; }
    // Só muda pelos métodos de transição abaixo; nunca de fora do agregado.
    public StatusConta Status { get; private set; }

    // Quem consome a conta pode LER o histórico, mas não alterar a lista diretamente.
    public IReadOnlyList<Transacao> Transacoes => _transacoes.AsReadOnly();

    // Construtor privado exclusivo para materialização do EF Core.
    private ContaBancaria()
    {
        Titular = null!;
        Saldo = null!;
        Status = StatusConta.Ativa;
    }

    public ContaBancaria(Titular titular) : base(ContaId.Novo())
    {
        // Titular sempre é válido por construção; só é preciso garantir que existe.
        ArgumentNullException.ThrowIfNull(titular);

        Titular = titular;
        Saldo = new Dinheiro(0);
    }

    public void Depositar(decimal valor)
    {
        GarantirContaAtiva();
        var dinheiro = new Dinheiro(valor);

        if (dinheiro.Valor <= 0)
            throw new ArgumentException("Valor de depósito deve ser maior que zero.");

        Saldo = Saldo + dinheiro; // usa o operador + do VO
        _transacoes.Add(new Transacao(TipoTransacao.Deposito, dinheiro));
    }

    public void Sacar(decimal valor)
    {
        GarantirContaAtiva();
        var dinheiro = new Dinheiro(valor);

        if (dinheiro.Valor <= 0)
            throw new ArgumentException("Valor de saque deve ser maior que zero.");

        if (Saldo < dinheiro) // usa o operador < do VO
            throw new SaldoInsuficienteException(Saldo.Valor, dinheiro.Valor);

        Saldo = Saldo - dinheiro;
        _transacoes.Add(new Transacao(TipoTransacao.Saque, dinheiro));
    }

    public void Bloquear()
    {
        if (Status != StatusConta.Ativa)
            throw new TransicaoStatusInvalidaException(Status, StatusConta.Bloqueada);

        Status = StatusConta.Bloqueada;
    }

    public void Desbloquear()
    {
        if (Status != StatusConta.Bloqueada)
            throw new TransicaoStatusInvalidaException(Status, StatusConta.Ativa);

        Status = StatusConta.Ativa;
    }

    public void Encerrar()
    {
        if (Status == StatusConta.Encerrada)
            throw new TransicaoStatusInvalidaException(Status, StatusConta.Encerrada);

        // Invariante: dinheiro não pode "sumir" com o encerramento da conta.
        if (Saldo.Valor != 0)
            throw new ContaComSaldoException(Saldo.Valor);

        Status = StatusConta.Encerrada;
    }

    // Toda movimentação financeira exige conta Ativa.
    private void GarantirContaAtiva()
    {
        if (Status != StatusConta.Ativa)
            throw new ContaNaoAtivaException(Status);
    }
}