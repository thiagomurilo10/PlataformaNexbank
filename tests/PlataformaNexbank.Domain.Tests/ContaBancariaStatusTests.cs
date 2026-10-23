using PlataformaNexbank.Domain.Entities;
using PlataformaNexbank.Domain.Enums;
using PlataformaNexbank.Domain.Exceptions;

namespace PlataformaNexbank.Domain.Tests; // ajuste ao namespace real

public class ContaBancariaStatusTests
{
    // Titular válido padrão, o mesmo usado em ContaBancariaTests
    private static ContaBancaria CriarConta() => new ContaBancaria(TitularBuilder.Padrao);

    [Fact]
    public void NovaConta_DeveIniciarAtiva()
    {
        var conta = CriarConta();

        Assert.Equal(StatusConta.Ativa, conta.Status);
    }

    [Fact]
    public void Bloquear_ContaAtiva_DeveFicarBloqueada()
    {
        var conta = CriarConta();

        conta.Bloquear();

        Assert.Equal(StatusConta.Bloqueada, conta.Status);
    }

    [Fact]
    public void Bloquear_ContaJaBloqueada_DeveLancarExcecao()
    {
        var conta = CriarConta();
        conta.Bloquear();

        Assert.Throws<TransicaoStatusInvalidaException>(() => conta.Bloquear());
    }

    [Fact]
    public void Desbloquear_ContaBloqueada_DeveFicarAtiva()
    {
        var conta = CriarConta();
        conta.Bloquear();

        conta.Desbloquear();

        Assert.Equal(StatusConta.Ativa, conta.Status);
    }

    [Fact]
    public void Desbloquear_ContaAtiva_DeveLancarExcecao()
    {
        var conta = CriarConta();

        Assert.Throws<TransicaoStatusInvalidaException>(() => conta.Desbloquear());
    }

    [Fact]
    public void Encerrar_ContaAtivaSemSaldo_DeveFicarEncerrada()
    {
        var conta = CriarConta();

        conta.Encerrar();

        Assert.Equal(StatusConta.Encerrada, conta.Status);
    }

    [Fact]
    public void Encerrar_ContaBloqueadaSemSaldo_DeveFicarEncerrada()
    {
        var conta = CriarConta();
        conta.Bloquear();

        conta.Encerrar();

        Assert.Equal(StatusConta.Encerrada, conta.Status);
    }

    [Fact]
    public void Encerrar_ContaComSaldo_DeveLancarExcecaoEManterAtiva()
    {
        var conta = CriarConta();
        conta.Depositar(100m);

        Assert.Throws<ContaComSaldoException>(() => conta.Encerrar());
        Assert.Equal(StatusConta.Ativa, conta.Status);
    }

    [Fact]
    public void Encerrar_ContaJaEncerrada_DeveLancarExcecao()
    {
        var conta = CriarConta();
        conta.Encerrar();

        Assert.Throws<TransicaoStatusInvalidaException>(() => conta.Encerrar());
    }

    [Fact]
    public void Bloquear_ContaEncerrada_DeveLancarExcecao()
    {
        var conta = CriarConta();
        conta.Encerrar();

        Assert.Throws<TransicaoStatusInvalidaException>(() => conta.Bloquear());
    }

    [Fact]
    public void Desbloquear_ContaEncerrada_DeveLancarExcecao()
    {
        var conta = CriarConta();
        conta.Encerrar();

        Assert.Throws<TransicaoStatusInvalidaException>(() => conta.Desbloquear());
    }

    [Fact]
    public void Depositar_ContaBloqueada_DeveLancarExcecaoSemAlterarSaldoNemHistorico()
    {
        var conta = CriarConta();
        conta.Bloquear();

        Assert.Throws<ContaNaoAtivaException>(() => conta.Depositar(50m));
        Assert.Equal(0m, conta.Saldo.Valor);
        Assert.Empty(conta.Transacoes);
    }

    [Fact]
    public void Sacar_ContaBloqueada_DeveLancarExcecaoSemAlterarSaldo()
    {
        var conta = CriarConta();
        conta.Depositar(100m);
        conta.Bloquear();

        Assert.Throws<ContaNaoAtivaException>(() => conta.Sacar(10m));
        Assert.Equal(100m, conta.Saldo.Valor);
    }

    [Fact]
    public void Depositar_ContaEncerrada_DeveLancarExcecao()
    {
        var conta = CriarConta();
        conta.Encerrar();

        Assert.Throws<ContaNaoAtivaException>(() => conta.Depositar(50m));
    }
}