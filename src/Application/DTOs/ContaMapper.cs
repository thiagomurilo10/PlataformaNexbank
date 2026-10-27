using PlataformaNexbank.Domain.Entities;

namespace PlataformaNexbank.Application.DTOs;

// Conversões manuais entre entidades de domínio e DTOs, evitando repetir a mesma lógica de mapeamento em cada use case.
public static class ContaMapper
{
    // Status exposto como texto ("Ativa", "Bloqueada", "Encerrada"): o cliente da API não depende do enum do domínio.
    public static ContaResponse ParaResponse(ContaBancaria conta) =>
        new(conta.Id.Valor,
            conta.Titular.Nome,
            conta.Titular.Cpf.Formatado,
            conta.Saldo.Valor,
            conta.Saldo.Moeda,
            conta.Status.ToString());

    public static TransacaoResponse ParaResponse(Transacao transacao) =>
        new(transacao.Tipo.ToString(), transacao.Valor.Valor, transacao.Valor.Moeda, transacao.Data);
}