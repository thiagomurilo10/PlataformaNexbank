using NexBank.Application.DTOs;
using NexBank.Application.UseCases;
using PlataformaNexbank.Application.Tests.Fakes;
using PlataformaNexbank.Domain.Entities;
using System;
using Xunit;

namespace PlataformaNexbank.Application.Tests.UseCases;

public class DepositarUseCaseTests
{
    [Fact]
    public void Executar_ComContaExistente_DeveAtualizarSaldoERetornarResponse()
    {
        var repositorio = new ContaBancariaRepositorioFake();
        var conta = new ContaBancaria("Thiago Murilo");
        repositorio.Adicionar(conta);

        var useCase = new DepositarUseCase(repositorio);
        var response = useCase.Executar(conta.Id, new DepositarRequest(100m));

        Assert.NotNull(response);
        Assert.Equal(100m, response!.Saldo);
    }

    [Fact]
    public void Executar_ComContaInexistente_DeveRetornarNull()
    {
        var repositorio = new ContaBancariaRepositorioFake();
        var useCase = new DepositarUseCase(repositorio);

        var response = useCase.Executar(Guid.NewGuid(), new DepositarRequest(100m));

        Assert.Null(response);
    }

    [Fact]
    public void Executar_ComValorInvalido_DevePropagarExcecaoDoDominio()
    {
        var repositorio = new ContaBancariaRepositorioFake();
        var conta = new ContaBancaria("Thiago Murilo");
        repositorio.Adicionar(conta);

        var useCase = new DepositarUseCase(repositorio);

        Assert.Throws<ArgumentException>(() =>
            useCase.Executar(conta.Id, new DepositarRequest(-10m)));
    }
}