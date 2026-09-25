using PlataformaNexbank.Domain.Entities;
using PlataformaNexbank.Domain.Repositories;
using System;


namespace PlataformaNexbank.Application.Tests.Fakes;

// Fake em memória — evita dependência de EF Core/banco real nos testes de use case.
public class ContaBancariaRepositorioFake : IContaBancariaRepositorio
{
    private readonly Dictionary<Guid, ContaBancaria> _contas = new();

    public void Adicionar(ContaBancaria conta) => _contas[conta.Id] = conta;

    public ContaBancaria? ObterPorId(Guid id) =>
        _contas.TryGetValue(id, out var conta) ? conta : null;

    public void Atualizar(ContaBancaria conta) => _contas[conta.Id] = conta;
}