using Microsoft.EntityFrameworkCore;
using PlataformaNexbank.Domain.Entities;

namespace PlataformaNexbank.Infrastructure.Persistence;

public class NexBankDbContext : DbContext
{
    public NexBankDbContext(DbContextOptions<NexBankDbContext> options)
        : base(options)
    {
    }

    public DbSet<ContaBancaria> Contas => Set<ContaBancaria>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NexBankDbContext).Assembly);
    }
}