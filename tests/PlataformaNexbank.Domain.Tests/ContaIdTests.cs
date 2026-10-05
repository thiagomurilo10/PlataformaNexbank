using PlataformaNexbank.Domain.ValueObjects;

namespace PlataformaNexbank.Domain.Tests.ValueObjects;

public class ContaIdTests
{
    [Fact]
    public void Novo_DeveGerarIdentificadorNaoVazio()
    {
        var id = ContaId.Novo();

        Assert.NotEqual(Guid.Empty, id.Valor);
    }

    [Fact]
    public void Novo_DeveGerarIdentificadoresDiferentes()
    {
        var primeiro = ContaId.Novo();
        var segundo = ContaId.Novo();

        Assert.NotEqual(primeiro, segundo);
    }

    [Fact]
    public void De_ComGuidValido_DeveCriarIdComMesmoValor()
    {
        var guid = Guid.NewGuid();

        var id = ContaId.De(guid);

        Assert.Equal(guid, id.Valor);
    }

    [Fact]
    public void De_ComGuidVazio_DeveLancarArgumentException()
    {
        Assert.Throws<ArgumentException>(() => ContaId.De(Guid.Empty));
    }

    [Fact]
    public void Igualdade_ComMesmoValor_DeveSerIgual()
    {
        var guid = Guid.NewGuid();

        var a = ContaId.De(guid);
        var b = ContaId.De(guid);

        Assert.Equal(a, b);
        Assert.True(a == b);
    }

    [Fact]
    public void ToString_DeveRetornarGuidComoTexto()
    {
        var guid = Guid.NewGuid();

        var id = ContaId.De(guid);

        Assert.Equal(guid.ToString(), id.ToString());
    }
}