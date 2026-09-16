namespace PlataformaNexbank.Domain.Exceptions;

// Exceção de domínio: lançada quando se tenta sacar mais do que o saldo disponível.
// Estou separando para deixar explícito que é uma regra de negócio violada
// facilitando o tratamento na camada de API (ex: retornar 422 em vez de 400).
public class SaldoInsuficienteException : Exception
{
    public SaldoInsuficienteException(decimal saldoAtual, decimal valorSolicitado)
        : base($"Saldo insuficiente. Saldo: {saldoAtual}, solicitado: {valorSolicitado}") { }
}