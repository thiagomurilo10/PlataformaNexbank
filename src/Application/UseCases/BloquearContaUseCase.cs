using PlataformaNexbank.Application.DTOs;
using PlataformaNexbank.Domain.Repositories;
using PlataformaNexbank.Domain.ValueObjects;

namespace PlataformaNexbank.Application.UseCases;

// Orquestra o bloqueio: busca o agregado, delega a regra a ele e persiste.
// A validação da transição fica no domínio (ContaBancaria.Bloquear), não aqui.
public class BloquearContaUseCase
{
    private readonly IContaBancariaRepositorio _repositorio;

    public BloquearContaUseCase(IContaBancariaRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    public ContaResponse? Executar(ContaId id)
    {
        var conta = _repositorio.ObterPorId(id);
        if (conta is null) return null;

        // Pode lançar TransicaoStatusInvalidaException: deixada propagar para o ApiExceptionHandler.
        conta.Bloquear();
        _repositorio.Atualizar(conta);

        return ContaMapper.ParaResponse(conta);
    }
}