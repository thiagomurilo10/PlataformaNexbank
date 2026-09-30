using PlataformaNexbank.Domain.ValueObjects;

namespace PlataformaNexbank.Domain.Tests;

public class TitularTests
{
    private static readonly Cpf CpfValido = Cpf.Criar("52998224725");

    [Fact]
    public void Criar_ComDadosValidos_PreencheNomeECpf()
    {
        var titular = Titular.Criar("Maria Silva", CpfValido);

        Assert.Equal("Maria Silva", titular.Nome);
        Assert.Equal(CpfValido, titular.Cpf);
    }

    [Fact]
    public void Criar_ComEspacosNasPontas_RemoveEspacosDoNome()
    {
        var titular = Titular.Criar("  Maria Silva  ", CpfValido);

        Assert.Equal("Maria Silva", titular.Nome);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Criar_ComNomeVazio_LancaArgumentException(string? nome)
    {
        Assert.Throws<ArgumentException>(() => Titular.Criar(nome, CpfValido));
    }

    [Theory]
    [InlineData("Jo")]
    [InlineData("  Jo  ")]
    public void Criar_ComNomeCurto_LancaArgumentException(string nome)
    {
        Assert.Throws<ArgumentException>(() => Titular.Criar(nome, CpfValido));
    }

    [Fact]
    public void Criar_ComNomeDeTresCaracteres_Aceita()
    {
        var titular = Titular.Criar("Ana", CpfValido);

        Assert.Equal("Ana", titular.Nome);
    }

    [Fact]
    public void Criar_ComCpfNulo_LancaArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Titular.Criar("Maria Silva", null!));
    }

    [Fact]
    public void Igualdade_TitularesComMesmosValores_SaoIguais()
    {
        var a = Titular.Criar("Maria Silva", Cpf.Criar("529.982.247-25"));
        var b = Titular.Criar("  Maria Silva ", Cpf.Criar("52998224725"));

        Assert.Equal(a, b);
    }

    [Fact]
    public void Igualdade_TitularesComCpfsDiferentes_NaoSaoIguais()
    {
        var a = Titular.Criar("Maria Silva", Cpf.Criar("52998224725"));
        var b = Titular.Criar("Maria Silva", Cpf.Criar("11144477735"));

        Assert.NotEqual(a, b);
    }
}