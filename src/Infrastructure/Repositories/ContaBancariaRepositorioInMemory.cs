using PlataformaNexbank.Domain.Entities;
using PlataformaNexbank.Domain.Repositories;

namespace PlataformaNexbank.Infrastructure.Repositories;

// Implementação concreta de IContaBancariaRepositorio (definida no Domain).
// Fica no Infrastructure porque é aqui que decisões técnicas de persistência
// (por enquanto in-memory, depois será EF Core/banco real) devem morar.
public class ContaBancariaRepositorioInMemory : IContaBancariaRepositorio
{
    // "Banco de dados" temporário: existe só enquanto a aplicação está rodando.
    // Dados são perdidos a cada reinício (aceitável nesta fase do projeto).
    private readonly Dictionary<Guid, ContaBancaria> _contas = new();

    public void Adicionar(ContaBancaria conta)
    {
        // Usa o Id da própria conta como chave do dicionário.
        _contas[conta.Id] = conta;
    }

    public ContaBancaria? ObterPorId(Guid id)
    {
        // TryGetValue evita exceção quando a chave não existe:
        // retorna true/false em vez de lançar erro, permitindo tratar
        // "não encontrado" como caso normal (retornando null).
        return _contas.TryGetValue(id, out var conta) ? conta : null;
    }
}