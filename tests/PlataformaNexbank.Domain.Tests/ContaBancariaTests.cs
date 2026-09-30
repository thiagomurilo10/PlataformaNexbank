using PlataformaNexbank.Domain.Entities;
using PlataformaNexbank.Domain.Enums;
using PlataformaNexbank.Domain.Exceptions;

namespace PlataformaNexbank.Domain.Tests;

public class ContaBancariaTests
{
    [Fact]
    public void Deve_Criar_Conta_Com_Saldo_Zero()
    {
        var conta = new ContaBancaria(TitularBuilder.Padrao);

        // Saldo inicial zero, titular armazenado e Id gerado.
        Assert.Equal(0, conta.Saldo.Valor);
        Assert.Equal(TitularBuilder.Padrao, conta.Titular);
        Assert.NotEqual(Guid.Empty, conta.Id);
    }

    [Fact]
    public void Deve_Lancar_Excecao_Quando_Titular_For_Nulo()
    {
        // Assert.Throws exige o tipo exato: ArgumentNullException, não ArgumentException.
        Assert.Throws<ArgumentNullException>(() => new ContaBancaria(null!));
    }

    // ==============================================
    // ==============================================

    [Fact]
    public void Deve_Somar_Valor_Ao_Saldo_Quando_Depositar_Valor_Valido()
    {
        var conta = new ContaBancaria(TitularBuilder.Padrao);

        conta.Depositar(100); // ação sendo testada

        Assert.Equal(100, conta.Saldo.Valor); // saldo deve refletir o depósito
    }

    [Theory]
    [InlineData(0)]         // valor zero não é depósito válido
    [InlineData(-1)]
    [InlineData(-100)]      // valores negativos também são inválidos
    public void Deve_Lancar_Excecao_Quando_Depositar_Valor_Invalido(decimal valor)
    {
        var conta = new ContaBancaria(TitularBuilder.Padrao);

        // depósito com valor <= 0 deve ser rejeitado pela regra de negócio
        Assert.Throws<ArgumentException>(() => conta.Depositar(valor));
    }

    // ==============================================
    // ==============================================

    [Fact]
    public void Deve_Subtrair_Valor_Do_Saldo_Quando_Sacar_Valor_Valido()
    {
        var conta = new ContaBancaria(TitularBuilder.Padrao);
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
        var conta = new ContaBancaria(TitularBuilder.Padrao);
        conta.Depositar(100);

        // saque com valor <= 0 deve ser rejeitado pela regra de negócio
        Assert.Throws<ArgumentException>(() => conta.Sacar(valor));
    }

    [Fact]
    public void Deve_Lancar_SaldoInsuficienteException_Quando_Sacar_Valor_Maior_Que_Saldo()
    {
        var conta = new ContaBancaria(TitularBuilder.Padrao);
        conta.Depositar(50);

        // regra de negócio: não é permitido saldo negativo
        Assert.Throws<SaldoInsuficienteException>(() => conta.Sacar(100));
    }

    // ==============================================
    // ==============================================

    [Fact]
    public void Deve_Registrar_Transacao_No_Historico_Quando_Depositar()
    {
        var conta = new ContaBancaria(TitularBuilder.Padrao);

        conta.Depositar(100);

        // valida que o depósito gerou um registro no histórico da conta
        var transacao = Assert.Single(conta.Transacoes);
        Assert.Equal(TipoTransacao.Deposito, transacao.Tipo);
        Assert.Equal(100, transacao.Valor.Valor);
    }

    [Fact]
    public void Deve_Registrar_Transacao_No_Historico_Quando_Sacar()
    {
        var conta = new ContaBancaria(TitularBuilder.Padrao);
        conta.Depositar(100);

        conta.Sacar(30);

        // deve haver 2 transações: o depósito e o saque, nessa ordem
        Assert.Equal(2, conta.Transacoes.Count);
        var saque = conta.Transacoes[1];
        Assert.Equal(TipoTransacao.Saque, saque.Tipo);
        Assert.Equal(30, saque.Valor.Valor);
    }
}