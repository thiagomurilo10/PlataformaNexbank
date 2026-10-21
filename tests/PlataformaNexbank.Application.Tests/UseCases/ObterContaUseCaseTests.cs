using PlataformaNexbank.Application.Tests.Fakes;
using PlataformaNexbank.Application.UseCases;
using PlataformaNexbank.Domain.Entities;
using PlataformaNexbank.Domain.Tests;
using PlataformaNexbank.Domain.ValueObjects;

namespace PlataformaNexbank.Application.Tests.UseCases;

public class ObterContaUseCaseTests
{
    [Fact]
    public void DeveRetornarContaExistente()
    {
        var conta = new ContaBancaria(TitularBuilder.Padrao);
        var repositorio = new ContaBancariaRepositorioFake();
        repositorio.Adicionar(conta);
        var useCase = new ObterContaUseCase(repositorio);

        var response = useCase.Executar(conta.Id);

        Assert.NotNull(response);
        Assert.Equal(conta.Id.Valor, response!.Id);
    }

    [Fact]
    public void DeveRetornarNuloQuandoContaNaoExiste()
    {
        var repositorio = new ContaBancariaRepositorioFake();
        var useCase = new ObterContaUseCase(repositorio);

        var response = useCase.Executar(ContaId.Novo());

        Assert.Null(response);
    }
}