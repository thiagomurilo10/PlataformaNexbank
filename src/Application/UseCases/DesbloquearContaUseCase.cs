using PlataformaNexbank.Application.DTOs;
using PlataformaNexbank.Domain.Repositories;
using PlataformaNexbank.Domain.ValueObjects;

namespace PlataformaNexbank.Application.UseCases;

// Orquestra o desbloqueio; a regra de transição fica em ContaBancaria.Desbloquear.
public class DesbloquearContaUseCase
{
    private readonly IContaBancariaRepositorio _repositorio;

    public DesbloquearContaUseCase(IContaBancariaRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    public ContaResponse? Executar(ContaId id)
    {
        var conta = _repositorio.ObterPorId(id);
        if (conta is null) return null;

        // Pode lançar TransicaoStatusInvalidaException: deixada propagar para o ApiExceptionHandler.
        conta.Desbloquear();
        _repositorio.Atualizar(conta);

        return ContaMapper.ParaResponse(conta);
    }
}