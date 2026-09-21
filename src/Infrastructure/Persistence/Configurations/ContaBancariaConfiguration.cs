using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaNexbank.Domain.Entities;

namespace PlataformaNexbank.Infrastructure.Persistence.Configurations;

public class ContaBancariaConfiguration : IEntityTypeConfiguration<ContaBancaria>
{
    public void Configure(EntityTypeBuilder<ContaBancaria> builder)
    {
        builder.ToTable("Contas");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Titular)
            .IsRequired()
            .HasMaxLength(200);

        // Dinheiro (Value Object) mapeado como Owned Type: sem tabela própria,
        // suas colunas (Valor, Moeda) viram colunas na própria tabela Contas.
        builder.OwnsOne(c => c.Saldo, saldo =>
        {
            saldo.Property(d => d.Valor)
                .HasColumnName("Saldo")
                .HasColumnType("decimal(18,2)");

            saldo.Property(d => d.Moeda)
                .HasColumnName("Moeda")
                .HasMaxLength(3);
        });

        // Transacoes: coleção owned, cada Transacao vira linha numa tabela própria
        // (TransacaoHistorico), vinculada à ContaBancaria por FK implícita.
        builder.OwnsMany(c => c.Transacoes, transacao =>
        {
            transacao.ToTable("TransacaoHistorico");

            transacao.WithOwner().HasForeignKey("ContaBancariaId");

            transacao.Property<int>("Id");
            transacao.HasKey("Id");

            transacao.Property(t => t.Tipo)
                .HasConversion<string>()
                .HasMaxLength(20);

            transacao.Property(t => t.Data);

            // Dinheiro aninhado dentro de Transacao: também Owned Type.
            transacao.OwnsOne(t => t.Valor, valor =>
            {
                valor.Property(d => d.Valor)
                    .HasColumnName("Valor")
                    .HasColumnType("decimal(18,2)");

                valor.Property(d => d.Moeda)
                    .HasColumnName("Moeda")
                    .HasMaxLength(3);
            });
        });

        // Acessa o campo privado _transacoes em vez da propriedade IReadOnlyList,
        // permitindo que o EF Core popule a lista diretamente.
        builder.Metadata.FindNavigation(nameof(ContaBancaria.Transacoes))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}