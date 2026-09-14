using PlataformaNexbank.Domain.Entities;

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
        Assert.Equal(0, conta.Saldo);
        Assert.Equal("João da Silva", conta.Titular);
    }

    // teste parametrizado: o mesmo corpo de teste roda várias vezes, apenas mudando o valor de entrada a cada teste.
    [Theory]
    [InlineData("")]      // string vazia
    [InlineData(" ")]     // string só com espaço (whitespace)
    [InlineData(null)]    // valor nulo
    public void Deve_Lancar_Excecao_Quando_Titular_Invalido(string titular)
    {
        // Assert.Throws executa a lambda e falha o teste se a exceção esperada (ArgumentException) NÃO for lançada.
        // Aqui valido a invariante de domínio: ContaBancaria nunca deve existir com titular inválido.
        Assert.Throws<ArgumentException>(() => new ContaBancaria(titular));
    }

    // ==============================================

    [Fact]
    public void Deve_Somar_Valor_Ao_Saldo_Quando_Depositar_Valor_Valido()
    {
        var conta = new ContaBancaria("João da Silva");

        conta.Depositar(100); // ação sendo testada

        Assert.Equal(100, conta.Saldo); // saldo deve refletir o depósito
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
}