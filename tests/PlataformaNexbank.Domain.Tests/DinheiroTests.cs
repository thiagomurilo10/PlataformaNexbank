using NexBank.Domain.ValueObjects;

namespace PlataformaNexbank.Domain.Tests;

public class DinheiroTests
{
    [Fact]
    public void Criar_ComValorNegativo_DeveLancarExcecao()
    {
        // Regra de negócio: dinheiro não pode ser negativo.
        Assert.Throws<ArgumentException>(() => new Dinheiro(-10));
    }

    [Fact]
    public void DoisDinheiros_ComMesmoValorEMoeda_DevemSerIguais()
    {
        // record garante igualdade estrutural (por valor, não por referência).
        var a = new Dinheiro(100, "BRL");
        var b = new Dinheiro(100, "BRL");

        Assert.Equal(a, b);
    }

    [Fact]
    public void Somar_MesmaMoeda_DeveSomarValores()
    {
        var a = new Dinheiro(100, "BRL");
        var b = new Dinheiro(50, "BRL");

        var resultado = a + b;

        Assert.Equal(150, resultado.Valor);
    }

    [Fact]
    public void Subtrair_MesmaMoeda_DeveSubtrairValores()
    {
        var a = new Dinheiro(100, "BRL");
        var b = new Dinheiro(30, "BRL");

        var resultado = a - b;

        Assert.Equal(70, resultado.Valor);
    }

    [Fact]
    public void Somar_MoedasDiferentes_DeveLancarExcecao()
    {
        // Impede erro silencioso de somar moedas incompatíveis.
        var a = new Dinheiro(100, "BRL");
        var b = new Dinheiro(50, "USD");

        Assert.Throws<InvalidOperationException>(() => { var _ = a + b; });
    }
}