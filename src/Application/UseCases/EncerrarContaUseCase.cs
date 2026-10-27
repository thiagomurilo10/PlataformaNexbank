using PlataformaNexbank.Application.DTOs;
using PlataformaNexbank.Domain.Repositories;
using PlataformaNexbank.Domain.ValueObjects;

namespace PlataformaNexbank.Application.UseCases;

// Orquestra o encerramento; as invariantes (saldo zero, não encerrada) ficam em ContaBancaria.Encerrar.
public class EncerrarContaUseCase
{
    private readonly IContaBancariaRepositorio _repositorio;

    public EncerrarContaUseCase(IContaBancariaRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    public ContaResponse? Executar(ContaId id)
    {
        var conta = _repositorio.ObterPorId(id);
        if (conta is null) return null;

        // Pode lançar TransicaoStatusInvalidaException ou ContaComSaldoException: deixadas propagar para o ApiExceptionHandler.
        conta.Encerrar();
        _repositorio.Atualizar(conta);

        return ContaMapper.ParaResponse(conta);
    }
}