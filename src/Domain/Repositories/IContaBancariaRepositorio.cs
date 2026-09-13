using PlataformaNexbank.Domain.Entities;

namespace PlataformaNexbank.Domain.Repositories;

public interface IContaBancariaRepositorio
{
    void Adicionar(ContaBancaria conta);
    ContaBancaria? ObterPorId(Guid id);
}