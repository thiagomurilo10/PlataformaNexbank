using PlataformaNexbank.Application.Tests.Fakes;
using PlataformaNexbank.Application.UseCases;
using PlataformaNexbank.Domain.Entities;

namespace PlataformaNexbank.Application.Tests.UseCases;

public class ListarTransacoesUseCaseTests
{
    [Fact]
    public void DeveRetornarTransacoesDaConta()
    {
        var conta = new ContaBancaria("Thiago");
        conta.Depositar(100m);
        var repositorio = new ContaBancariaRepositorioFake();
        repositorio.Adicionar(conta);
        var useCase = new ListarTransacoesUseCase(repositorio);

        var response = useCase.Executar(conta.Id);

        Assert.NotNull(response);
        Assert.Single(response!);
    }

    [Fact]
    public void DeveRetornarNuloQuandoContaNaoExiste()
    {
        var repositorio = new ContaBancariaRepositorioFake();
        var useCase = new ListarTransacoesUseCase(repositorio);

        var response = useCase.Executar(Guid.NewGuid());

        Assert.Null(response);
    }
}