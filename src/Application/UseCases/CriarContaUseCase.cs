using PlataformaNexbank.Application.DTOs;
using PlataformaNexbank.Domain.Entities;
using PlataformaNexbank.Domain.Repositories;
using PlataformaNexbank.Domain.ValueObjects;

namespace PlataformaNexbank.Application.UseCases;

public class CriarContaUseCase
{
    private readonly IContaBancariaRepositorio _repositorio;

    public CriarContaUseCase(IContaBancariaRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    public ContaResponse Executar(CriarContaRequest request)
    {
        // Cpf.Criar e Titular.Criar validam; dado inválido lança ArgumentException (→ 400).
        var cpf = Cpf.Criar(request.Cpf);
        var titular = Titular.Criar(request.Nome, cpf);

        var conta = new ContaBancaria(titular);
        _repositorio.Adicionar(conta);
        return ContaMapper.ParaResponse(conta);
    }
}