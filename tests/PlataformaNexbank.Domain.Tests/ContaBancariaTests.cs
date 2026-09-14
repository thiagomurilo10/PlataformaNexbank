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
}