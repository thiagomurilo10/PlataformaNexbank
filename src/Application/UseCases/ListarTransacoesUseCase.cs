using NexBank.Application.DTOs;
using PlataformaNexbank.Domain.Repositories;

namespace NexBank.Application.UseCases;

public class ListarTransacoesUseCase
{
    private readonly IContaBancariaRepositorio _repositorio;

    public ListarTransacoesUseCase(IContaBancariaRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    public IReadOnlyList<TransacaoResponse>? Executar(Guid id)
    {
        var conta = _repositorio.ObterPorId(id);
        if (conta is null) return null;

        return conta.Transacoes.Select(ContaMapper.ParaResponse).ToList();
    }
}