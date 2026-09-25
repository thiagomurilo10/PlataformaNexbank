using PlataformaNexbank.Domain.Entities;

namespace PlataformaNexbank.Domain.Repositories;

public interface IContaBancariaRepositorio
{
    void Adicionar(ContaBancaria conta);
    ContaBancaria? ObterPorId(Guid id);
    void Atualizar(ContaBancaria conta); // persiste alterações em entidade já existente

}