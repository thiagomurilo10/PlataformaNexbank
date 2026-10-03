using PlataformaNexbank.Domain.ValueObjects;

namespace PlataformaNexbank.Domain.Tests;

public class CpfTests
{
    [Theory]
    [InlineData("52998224725")]
    [InlineData("529.982.247-25")]
    [InlineData("  529.982.247-25  ")]
    [InlineData("11144477735")]
    public void Criar_ComCpfValido_NormalizaParaApenasDigitos(string entrada)
    {
        var cpf = Cpf.Criar(entrada);

        Assert.Equal(11, cpf.Numero.Length);
        Assert.All(cpf.Numero, c => Assert.True(char.IsAsciiDigit(c)));
    }

    [Fact]
    public void Formatado_RetornaCpfComMascara()
    {
        var cpf = Cpf.Criar("52998224725");

        Assert.Equal("529.982.247-25", cpf.Formatado);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Criar_ComValorVazio_LancaArgumentException(string? entrada)
    {
        Assert.Throws<ArgumentException>(() => Cpf.Criar(entrada));
    }

    [Theory]
    [InlineData("1234567890")]
    [InlineData("123456789012")]
    [InlineData("abc")]
    public void Criar_ComTamanhoInvalido_LancaArgumentException(string entrada)
    {
        Assert.Throws<ArgumentException>(() => Cpf.Criar(entrada));
    }

    [Theory]
    [InlineData("00000000000")]
    [InlineData("11111111111")]
    [InlineData("99999999999")]
    public void Criar_ComSequenciaRepetida_LancaArgumentException(string entrada)
    {
        Assert.Throws<ArgumentException>(() => Cpf.Criar(entrada));
    }

    [Theory]
    [InlineData("52998224726")]
    [InlineData("52998224735")]
    [InlineData("11144477734")]
    public void Criar_ComDigitoVerificadorInvalido_LancaArgumentException(string entrada)
    {
        Assert.Throws<ArgumentException>(() => Cpf.Criar(entrada));
    }

    [Fact]
    public void Igualdade_CpfsComMesmoNumero_SaoIguais()
    {
        var comMascara = Cpf.Criar("529.982.247-25");
        var semMascara = Cpf.Criar("52998224725");

        Assert.Equal(comMascara, semMascara);
        Assert.Equal(comMascara.GetHashCode(), semMascara.GetHashCode());
    }

    [Fact]
    public void Igualdade_CpfsDiferentes_NaoSaoIguais()
    {
        Assert.NotEqual(Cpf.Criar("52998224725"), Cpf.Criar("11144477735"));
    }
}