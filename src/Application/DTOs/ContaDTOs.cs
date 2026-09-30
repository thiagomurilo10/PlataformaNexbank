namespace PlataformaNexbank.Application.DTOs;

// DTO de entrada para criação de conta 
public record CriarContaRequest(string Nome, string Cpf);

// DTO de saída — expõe apenas tipos primitivos, nunca o Value Object Dinheiro do domínio.
public record ContaResponse(
    Guid Id,
    string Titular,
    string Cpf,
    decimal Saldo,
    string Moeda);

// DTO de entrada para depósito.
public record DepositarRequest(decimal Valor);

// DTO de entrada para saque.
public record SacarRequest(decimal Valor);

// DTO de saída para cada item do histórico de transações.
public record TransacaoResponse(
    string Tipo,
    decimal Valor,
    string Moeda,
    DateTime Data);