using PlataformaNexbank.Domain.Entities;
using PlataformaNexbank.Domain.ValueObjects;

namespace PlataformaNexbank.Domain.Repositories;

public interface IContaBancariaRepositorio
{
    void Adicionar(ContaBancaria conta);
    ContaBancaria? ObterPorId(ContaId id);
    bool ExisteCpf(Cpf cpf); // verifica unicidade antes de criar a conta
    void Atualizar(ContaBancaria conta); // persiste alterações em entidade já existente

}