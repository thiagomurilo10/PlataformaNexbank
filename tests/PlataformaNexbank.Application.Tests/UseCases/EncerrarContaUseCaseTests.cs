using PlataformaNexbank.Application.Tests.Fakes;
using PlataformaNexbank.Application.UseCases;
using PlataformaNexbank.Domain.Entities;
using PlataformaNexbank.Domain.Enums;
using PlataformaNexbank.Domain.Exceptions;
using PlataformaNexbank.Domain.Tests;
using PlataformaNexbank.Domain.ValueObjects;

namespace PlataformaNexbank.Application.Tests.UseCases;

public class EncerrarContaUseCaseTests
{
    [Fact]
    public void Executar_ComContaSemSaldo_DeveEncerrarPersistirERetornarResponse()
    {
        var repositorio = new ContaBancariaRepositorioFake();
        var conta = new ContaBancaria(TitularBuilder.Padrao);
        repositorio.Adicionar(conta);

        var response = new EncerrarContaUseCase(repositorio).Executar(conta.Id);

        Assert.NotNull(response);
        Assert.Equal("Encerrada", response!.Status);
        Assert.Equal(StatusConta.Encerrada, repositorio.ObterPorId(conta.Id)!.Status);
    }

    [Fact]
    public void Executar_ComContaInexistente_DeveRetornarNull()
    {
        var repositorio = new ContaBancariaRepositorioFake();

        var response = new EncerrarContaUseCase(repositorio).Executar(ContaId.Novo());

        Assert.Null(response);
    }

    [Fact]
    public void Executar_ComContaComSaldo_DevePropagarExcecaoEManterAtiva()
    {
        var repositorio = new ContaBancariaRepositorioFake();
        var conta = new ContaBancaria(TitularBuilder.Padrao);
        conta.Depositar(100m);
        repositorio.Adicionar(conta);

        var useCase = new EncerrarContaUseCase(repositorio);

        Assert.Throws<ContaComSaldoException>(() => useCase.Executar(conta.Id));
        Assert.Equal(StatusConta.Ativa, repositorio.ObterPorId(conta.Id)!.Status);
    }
}