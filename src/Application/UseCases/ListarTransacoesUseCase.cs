using PlataformaNexbank.Application.DTOs;
using PlataformaNexbank.Domain.Repositories;
using PlataformaNexbank.Domain.ValueObjects;

namespace PlataformaNexbank.Application.UseCases;

public class ListarTransacoesUseCase
{
    private readonly IContaBancariaRepositorio _repositorio;

    public ListarTransacoesUseCase(IContaBancariaRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    public IReadOnlyList<TransacaoResponse>? Executar(ContaId id)
    {
        var conta = _repositorio.ObterPorId(id);
        if (conta is null) return null;

        return conta.Transacoes.Select(ContaMapper.ParaResponse).ToList();
    }
}