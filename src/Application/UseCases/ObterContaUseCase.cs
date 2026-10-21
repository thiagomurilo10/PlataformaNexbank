using PlataformaNexbank.Application.DTOs;
using PlataformaNexbank.Domain.Repositories;
using PlataformaNexbank.Domain.ValueObjects;

namespace PlataformaNexbank.Application.UseCases;

public class ObterContaUseCase
{
    private readonly IContaBancariaRepositorio _repositorio;

    public ObterContaUseCase(IContaBancariaRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    // Retorna null quando a conta não existe — quem chama decide o status HTTP (404).
    public ContaResponse? Executar(ContaId id)
    {
        var conta = _repositorio.ObterPorId(id);
        return conta is null ? null : ContaMapper.ParaResponse(conta);
    }
}