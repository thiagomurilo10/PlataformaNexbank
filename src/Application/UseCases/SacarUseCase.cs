using NexBank.Application.DTOs;
using PlataformaNexbank.Domain.Repositories;

namespace NexBank.Application.UseCases;

public class SacarUseCase
{
    private readonly IContaBancariaRepositorio _repositorio;

    public SacarUseCase(IContaBancariaRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    public ContaResponse? Executar(Guid id, SacarRequest request)
    {
        var conta = _repositorio.ObterPorId(id);
        if (conta is null) return null;

        conta.Sacar(request.Valor); // pode lançar ArgumentException ou SaldoInsuficienteException
        _repositorio.Atualizar(conta);

        return ContaMapper.ParaResponse(conta);
    }
}