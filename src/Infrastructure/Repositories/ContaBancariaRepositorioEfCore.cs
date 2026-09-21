using Microsoft.EntityFrameworkCore;
using PlataformaNexbank.Domain.Entities;
using PlataformaNexbank.Domain.Repositories;
using PlataformaNexbank.Infrastructure.Persistence;

namespace PlataformaNexbank.Infrastructure.Repositories;

// Implementação concreta de IContaBancariaRepositorio usando EF Core + PostgreSQL.
// Substitui a versão in-memory: dados agora persistem entre reinícios da aplicação.
public class ContaBancariaRepositorioEfCore : IContaBancariaRepositorio
{
    private readonly NexBankDbContext _dbContext;

    public ContaBancariaRepositorioEfCore(NexBankDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Adicionar(ContaBancaria conta)
    {
        _dbContext.Contas.Add(conta);
        _dbContext.SaveChanges();
    }

    public ContaBancaria? ObterPorId(Guid id)
    {
        // Include necessário: por padrão o EF Core não carrega coleções owned
        // automaticamente (lazy loading não está habilitado neste projeto).
        return _dbContext.Contas
            .Include(c => c.Transacoes)
            .FirstOrDefault(c => c.Id == id);
    }
}