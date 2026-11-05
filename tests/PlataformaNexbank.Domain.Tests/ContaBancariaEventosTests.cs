using PlataformaNexbank.Domain.Entities;
using PlataformaNexbank.Domain.Events;
using PlataformaNexbank.Domain.Exceptions;

namespace PlataformaNexbank.Domain.Tests;

public class ContaBancariaEventosTests
{
    private static ContaBancaria NovaConta() => new(TitularBuilder.Padrao);

    [Fact]
    public void Criar_RegistraContaCriada()
    {
        var conta = NovaConta();

        var evento = Assert.IsType<ContaCriada>(Assert.Single(conta.DomainEvents));
        Assert.Equal(conta.Id, evento.ContaId);
    }

    [Fact]
    public void Criar_TitularNulo_LancaExcecaoSemRegistrarEvento()
    {
        Assert.Throws<ArgumentNullException>(() => new ContaBancaria(null!));
    }

    [Fact]
    public void Depositar_RegistraDepositoRealizado()
    {
        var conta = NovaConta();
        conta.LimparEventos();

        conta.Depositar(100m);

        var evento = Assert.IsType<DepositoRealizado>(Assert.Single(conta.DomainEvents));
        Assert.Equal(conta.Id, evento.ContaId);
        Assert.Equal(100m, evento.Valor.Valor);
        Assert.Equal(conta.Saldo, evento.SaldoApos);
    }

    [Fact]
    public void Sacar_RegistraSaqueRealizado()
    {
        var conta = NovaConta();
        conta.Depositar(200m);
        conta.LimparEventos();

        conta.Sacar(50m);

        var evento = Assert.IsType<SaqueRealizado>(Assert.Single(conta.DomainEvents));
        Assert.Equal(conta.Id, evento.ContaId);
        Assert.Equal(50m, evento.Valor.Valor);
        Assert.Equal(conta.Saldo, evento.SaldoApos);
        Assert.Equal(150m, evento.SaldoApos.Valor);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Depositar_ValorInvalido_NaoRegistraEvento(decimal valor)
    {
        var conta = NovaConta();
        conta.LimparEventos();

        Assert.Throws<ArgumentException>(() => conta.Depositar(valor));

        Assert.Empty(conta.DomainEvents);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Sacar_ValorInvalido_NaoRegistraEvento(decimal valor)
    {
        var conta = NovaConta();
        conta.Depositar(100m);
        conta.LimparEventos();

        Assert.Throws<ArgumentException>(() => conta.Sacar(valor));

        Assert.Empty(conta.DomainEvents);
    }

    [Fact]
    public void Sacar_SaldoInsuficiente_NaoRegistraEvento()
    {
        var conta = NovaConta();
        conta.LimparEventos();

        Assert.Throws<SaldoInsuficienteException>(() => conta.Sacar(10m));

        Assert.Empty(conta.DomainEvents);
    }

    [Fact]
    public void Depositar_ContaBloqueada_NaoRegistraEvento()
    {
        var conta = NovaConta();
        conta.Bloquear();
        conta.LimparEventos();

        Assert.ThrowsAny<DomainException>(() => conta.Depositar(100m));

        Assert.Empty(conta.DomainEvents);
    }

    [Fact]
    public void Bloquear_RegistraContaBloqueada()
    {
        var conta = NovaConta();
        conta.LimparEventos();

        conta.Bloquear();

        var evento = Assert.IsType<ContaBloqueada>(Assert.Single(conta.DomainEvents));
        Assert.Equal(conta.Id, evento.ContaId);
    }

    [Fact]
    public void Desbloquear_RegistraContaDesbloqueada()
    {
        var conta = NovaConta();
        conta.Bloquear();
        conta.LimparEventos();

        conta.Desbloquear();

        var evento = Assert.IsType<ContaDesbloqueada>(Assert.Single(conta.DomainEvents));
        Assert.Equal(conta.Id, evento.ContaId);
    }

    [Fact]
    public void Encerrar_RegistraContaEncerrada()
    {
        var conta = NovaConta();
        conta.LimparEventos();

        conta.Encerrar();

        var evento = Assert.IsType<ContaEncerrada>(Assert.Single(conta.DomainEvents));
        Assert.Equal(conta.Id, evento.ContaId);
    }

    [Fact]
    public void Bloquear_ContaEncerrada_NaoRegistraEvento()
    {
        var conta = NovaConta();
        conta.Encerrar();
        conta.LimparEventos();

        Assert.ThrowsAny<DomainException>(() => conta.Bloquear());

        Assert.Empty(conta.DomainEvents);
    }

    [Fact]
    public void Encerrar_ComSaldo_NaoRegistraEvento()
    {
        var conta = NovaConta();
        conta.Depositar(100m);
        conta.LimparEventos();

        Assert.ThrowsAny<DomainException>(() => conta.Encerrar());

        Assert.Empty(conta.DomainEvents);
    }
}