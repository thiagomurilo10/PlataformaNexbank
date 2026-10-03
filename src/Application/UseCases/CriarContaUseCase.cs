using PlataformaNexbank.Application.DTOs;
using PlataformaNexbank.Domain.Entities;
using PlataformaNexbank.Domain.Exceptions;
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

        // Regra de unicidade: depende do repositório, por isso fica no use case e não no VO.
        if (_repositorio.ExisteCpf(cpf))
            throw new CpfJaCadastradoException();

        var conta = new ContaBancaria(titular);
        _repositorio.Adicionar(conta);
        return ContaMapper.ParaResponse(conta);
    }
}