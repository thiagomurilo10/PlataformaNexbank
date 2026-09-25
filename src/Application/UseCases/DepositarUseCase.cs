using NexBank.Application.DTOs;
using PlataformaNexbank.Domain.Repositories;

namespace NexBank.Application.UseCases;

// Resultado explícito em vez de lançar exceção para "conta não encontrada":
// deixa claro no tipo de retorno que esse caso existe, sem forçar o chamador a usar try/catch.
public class DepositarUseCase
{
    private readonly IContaBancariaRepositorio _repositorio;

    public DepositarUseCase(IContaBancariaRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    public ContaResponse? Executar(Guid id, DepositarRequest request)
    {
        var conta = _repositorio.ObterPorId(id);
        if (conta is null) return null;

        conta.Depositar(request.Valor); // pode lançar ArgumentException — deixado propagar de propósito
        _repositorio.Atualizar(conta);

        return ContaMapper.ParaResponse(conta);
    }
}