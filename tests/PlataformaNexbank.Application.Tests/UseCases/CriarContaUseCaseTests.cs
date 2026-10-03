using PlataformaNexbank.Application.DTOs;
using PlataformaNexbank.Application.Tests.Fakes;
using PlataformaNexbank.Application.UseCases;
using PlataformaNexbank.Domain.Exceptions;

namespace PlataformaNexbank.Application.Tests.UseCases;

public class CriarContaUseCaseTests
{
    private readonly ContaBancariaRepositorioFake _repositorio = new();
    private readonly CriarContaUseCase _useCase;

    public CriarContaUseCaseTests()
    {
        _useCase = new CriarContaUseCase(_repositorio);
    }

    [Fact]
    public void Executar_ComDadosValidos_CriaEPersisteConta()
    {
        var response = _useCase.Executar(new CriarContaRequest("Maria Silva", "529.982.247-25"));

        Assert.Equal("Maria Silva", response.Titular);
        Assert.Equal("529.982.247-25", response.Cpf);
        Assert.Equal(0, response.Saldo);
        Assert.NotNull(_repositorio.ObterPorId(response.Id));
    }

    [Theory]
    [InlineData("529.982.247-25")]
    [InlineData("52998224725")] // mesmo CPF sem máscara: a unicidade vale pelo número
    public void Executar_ComCpfJaCadastrado_LancaCpfJaCadastradoException(string cpfDuplicado)
    {
        _useCase.Executar(new CriarContaRequest("Maria Silva", "529.982.247-25"));

        Assert.Throws<CpfJaCadastradoException>(() =>
            _useCase.Executar(new CriarContaRequest("Outra Pessoa", cpfDuplicado)));
    }

    [Theory]
    [InlineData("11111111111")]
    [InlineData("123")]
    [InlineData("")]
    public void Executar_ComCpfInvalido_LancaArgumentException(string cpfInvalido)
    {
        Assert.Throws<ArgumentException>(() =>
            _useCase.Executar(new CriarContaRequest("Maria Silva", cpfInvalido)));
    }

    [Fact]
    public void Executar_ComNomeCurto_LancaArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            _useCase.Executar(new CriarContaRequest("Jo", "529.982.247-25")));
    }
}