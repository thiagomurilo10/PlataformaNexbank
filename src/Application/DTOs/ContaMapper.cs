using PlataformaNexbank.Domain.Entities;

namespace NexBank.Application.DTOs;

// Conversões manuais entre entidades de domínio e DTOs, evitando repetir a mesma lógica de mapeamento em cada use case.
public static class ContaMapper
{
    public static ContaResponse ParaResponse(ContaBancaria conta) =>
        new(conta.Id, conta.Titular, conta.Saldo.Valor, conta.Saldo.Moeda);

    public static TransacaoResponse ParaResponse(Transacao transacao) =>
        new(transacao.Tipo.ToString(), transacao.Valor.Valor, transacao.Valor.Moeda, transacao.Data);
}