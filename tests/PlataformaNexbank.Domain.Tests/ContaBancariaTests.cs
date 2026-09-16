using PlataformaNexbank.Domain.Entities;
using PlataformaNexbank.Domain.Enums;
using PlataformaNexbank.Domain.Exceptions;

namespace PlataformaNexbank.Domain.Tests;

public class ContaBancariaTests
{
    // testando com um único cenário fixo, sem parâmetros.
    [Fact]
    public void Deve_Criar_Conta_Com_Saldo_Zero()
    {
        // cria a conta (construtor já executa a regra de negócio).
        var conta = new ContaBancaria("João da Silva");

        // verifica se o construtor cumpriu o contrato esperado
        // saldo inicial zero e titular armazenado corretamente.
        Assert.Equal(0, conta.Saldo.Valor);
        Assert.Equal("João da Silva", conta.Titular);
    }

    // teste parametrizado: o mesmo corpo de teste roda várias vezes, apenas mudando o valor de entrada a cada teste.
    [Theory]
    [InlineData("")]      // string vazia
    [InlineData(" ")]     // string só com espaço (whitespace)
    [InlineData(null)]    // valor nulo
    public void Deve_Lancar_Excecao_Quando_Titular_Invalido(string? titular)
    {
        // Assert.Throws executa a lambda e falha o teste se a exceção esperada (ArgumentException) NÃO for lançada.
        // Aqui valido a invariante de domínio: ContaBancaria nunca deve existir com titular inválido.
        Assert.Throws<ArgumentException>(() => new ContaBancaria(titular!));
    }

    // ==============================================
    // ==============================================

    [Fact]
    public void Deve_Somar_Valor_Ao_Saldo_Quando_Depositar_Valor_Valido()
    {
        var conta = new ContaBancaria("João da Silva");

        conta.Depositar(100); // ação sendo testada

        Assert.Equal(100, conta.Saldo.Valor); // saldo deve refletir o depósito
    }

    [Theory]
    [InlineData(0)]         // valor zero não é depósito válido
    [InlineData(-1)]
    [InlineData(-100)]      // valores negativos também são inválidos
    public void Deve_Lancar_Excecao_Quando_Depositar_Valor_Invalido(decimal valor)
    {
        var conta = new ContaBancaria("João da Silva");

        // depósito com valor <= 0 deve ser rejeitado pela regra de negócio
        Assert.Throws<ArgumentException>(() => conta.Depositar(valor));
    }

    // ==============================================
    // ==============================================

    [Fact]
    public void Deve_Subtrair_Valor_Do_Saldo_Quando_Sacar_Valor_Valido()
    {
        var conta = new ContaBancaria("João da Silva");
        conta.Depositar(100);

        conta.Sacar(40); // ação sendo testada

        Assert.Equal(60, conta.Saldo.Valor); // saldo deve refletir o saque
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Deve_Lancar_ArgumentException_Quando_Sacar_Valor_Invalido(decimal valor)
    {
        var conta = new ContaBancaria("João da Silva");
        conta.Depositar(100);

        // saque com valor <= 0 deve ser rejeitado pela regra de negócio
        Assert.Throws<ArgumentException>(() => conta.Sacar(valor));
    }

    [Fact]
    public void Deve_Lancar_SaldoInsuficienteException_Quando_Sacar_Valor_Maior_Que_Saldo()
    {
        var conta = new ContaBancaria("João da Silva");
        conta.Depositar(50);

        // regra de negócio: não é permitido saldo negativo
        Assert.Throws<SaldoInsuficienteException>(() => conta.Sacar(100));
    }

    // ==============================================
    // ==============================================

    [Fact]
    public void Deve_Registrar_Transacao_No_Historico_Quando_Depositar()
    {
        var conta = new ContaBancaria("João da Silva");

        conta.Depositar(100);

        // valida que o depósito gerou um registro no histórico da conta
        var transacao = Assert.Single(conta.Transacoes);
        Assert.Equal(TipoTransacao.Deposito, transacao.Tipo);
        Assert.Equal(100, transacao.Valor.Valor);
    }

    [Fact]
    public void Deve_Registrar_Transacao_No_Historico_Quando_Sacar()
    {
        var conta = new ContaBancaria("João da Silva");
        conta.Depositar(100);

        conta.Sacar(30);

        // deve haver 2 transações: o depósito e o saque, nessa ordem
        Assert.Equal(2, conta.Transacoes.Count);
        var saque = conta.Transacoes[1];
        Assert.Equal(TipoTransacao.Saque, saque.Tipo);
        Assert.Equal(30, saque.Valor.Valor);
    }
}