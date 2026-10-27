using PlataformaNexbank.Application.DTOs;
using PlataformaNexbank.Domain.Repositories;
using PlataformaNexbank.Domain.ValueObjects;

namespace PlataformaNexbank.Application.UseCases;

// Resultado explícito em vez de lançar exceção para "conta não encontrada":
// deixa claro no tipo de retorno que esse caso existe, sem forçar o chamador a usar try/catch.
public class DepositarUseCase
{
    private readonly IContaBancariaRepositorio _repositorio;

    public DepositarUseCase(IContaBancariaRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    public ContaResponse? Executar(ContaId id, DepositarRequest request)
    {
        var conta = _repositorio.ObterPorId(id);
        if (conta is null) return null;

        // Pode lançar ArgumentException ou ContaNaoAtivaException: deixadas propagar para o ApiExceptionHandler.
        conta.Depositar(request.Valor);
        _repositorio.Atualizar(conta);

        return ContaMapper.ParaResponse(conta);
    }
}