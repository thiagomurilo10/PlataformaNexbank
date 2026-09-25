using NexBank.Application.DTOs;
using NexBank.Application.UseCases;
using PlataformaNexbank.Application.Tests.Fakes;
using PlataformaNexbank.Domain.Entities;
using PlataformaNexbank.Domain.Exceptions;
using Xunit;


namespace PlataformaNexbank.Application.Tests.UseCases;

public class SacarUseCaseTests
{
    [Fact]
    public void Executar_ComSaldoSuficiente_DeveAtualizarSaldoERetornarResponse()
    {
        var repositorio = new ContaBancariaRepositorioFake();
        var conta = new ContaBancaria("Thiago Murilo");
        conta.Depositar(100m);
        repositorio.Adicionar(conta);

        var useCase = new SacarUseCase(repositorio);
        var response = useCase.Executar(conta.Id, new SacarRequest(30m));

        Assert.NotNull(response);
        Assert.Equal(70m, response!.Saldo);
    }

    [Fact]
    public void Executar_ComSaldoInsuficiente_DevePropagarExcecao()
    {
        var repositorio = new ContaBancariaRepositorioFake();
        var conta = new ContaBancaria("Thiago Murilo");
        repositorio.Adicionar(conta);

        var useCase = new SacarUseCase(repositorio);

        Assert.Throws<SaldoInsuficienteException>(() =>
            useCase.Executar(conta.Id, new SacarRequest(50m)));
    }

    [Fact]
    public void Executar_ComContaInexistente_DeveRetornarNull()
    {
        var repositorio = new ContaBancariaRepositorioFake();
        var useCase = new SacarUseCase(repositorio);

        var response = useCase.Executar(Guid.NewGuid(), new SacarRequest(10m));

        Assert.Null(response);
    }
}