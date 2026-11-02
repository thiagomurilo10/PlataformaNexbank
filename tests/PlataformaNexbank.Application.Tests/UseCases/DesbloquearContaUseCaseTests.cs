using PlataformaNexbank.Application.Tests.Fakes;
using PlataformaNexbank.Application.UseCases;
using PlataformaNexbank.Domain.Entities;
using PlataformaNexbank.Domain.Enums;
using PlataformaNexbank.Domain.Exceptions;
using PlataformaNexbank.Domain.Tests;
using PlataformaNexbank.Domain.ValueObjects;

namespace PlataformaNexbank.Application.Tests.UseCases;

public class DesbloquearContaUseCaseTests
{
    [Fact]
    public void Executar_ComContaBloqueada_DeveDesbloquearPersistirERetornarResponse()
    {
        var repositorio = new ContaBancariaRepositorioFake();
        var conta = new ContaBancaria(TitularBuilder.Padrao);
        conta.Bloquear();
        repositorio.Adicionar(conta);

        var response = new DesbloquearContaUseCase(repositorio).Executar(conta.Id);

        Assert.NotNull(response);
        Assert.Equal("Ativa", response!.Status);
        Assert.Equal(StatusConta.Ativa, repositorio.ObterPorId(conta.Id)!.Status);
    }

    [Fact]
    public void Executar_ComContaInexistente_DeveRetornarNull()
    {
        var repositorio = new ContaBancariaRepositorioFake();

        var response = new DesbloquearContaUseCase(repositorio).Executar(ContaId.Novo());

        Assert.Null(response);
    }

    [Fact]
    public void Executar_ComContaAtiva_DevePropagarExcecaoDoDominio()
    {
        var repositorio = new ContaBancariaRepositorioFake();
        var conta = new ContaBancaria(TitularBuilder.Padrao);
        repositorio.Adicionar(conta);

        var useCase = new DesbloquearContaUseCase(repositorio);

        Assert.Throws<TransicaoStatusInvalidaException>(() => useCase.Executar(conta.Id));
    }
}