using NexBank.Application.DTOs;
using PlataformaNexbank.Domain.Entities;
using PlataformaNexbank.Domain.Repositories;

namespace NexBank.Application.UseCases;

public class CriarContaUseCase
{
    private readonly IContaBancariaRepositorio _repositorio;

    public CriarContaUseCase(IContaBancariaRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    public ContaResponse Executar(CriarContaRequest request)
    {
        var conta = new ContaBancaria(request.Titular);
        _repositorio.Adicionar(conta);
        return ContaMapper.ParaResponse(conta);
    }
}