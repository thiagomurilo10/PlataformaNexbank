using Microsoft.EntityFrameworkCore;
using PlataformaNexbank.Domain.Entities;
using PlataformaNexbank.Domain.Exceptions;
using PlataformaNexbank.Domain.Repositories;
using PlataformaNexbank.Infrastructure.Persistence;
using PlataformaNexbank.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Registra o repositório EF Core como implementação da interface do Domain.
// Scoped: uma instância por requisição HTTP, alinhada ao ciclo de vida do DbContext (que também é Scoped) — necessário para evitar captive dependency.
builder.Services.AddScoped<IContaBancariaRepositorio, ContaBancariaRepositorioEfCore>();


var connectionString = builder.Configuration.GetConnectionString("NexBankDb");

builder.Services.AddDbContext<NexBankDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Cria uma conta bancária nova, com saldo zero, a partir do titular informado.
app.MapPost("/contas", (CriarContaRequest request, IContaBancariaRepositorio repositorio) =>
{
    var conta = new ContaBancaria(request.Titular);
    repositorio.Adicionar(conta);

    // 201 Created + header Location apontando para o recurso recém-criado.
    return Results.Created($"/contas/{conta.Id}", conta);
});

// Busca uma conta pelo Id. {id:guid} rejeita valores que não sejam Guid válido.
app.MapGet("/contas/{id:guid}", (Guid id, IContaBancariaRepositorio repositorio) =>
{
    var conta = repositorio.ObterPorId(id);
    return conta is not null ? Results.Ok(conta) : Results.NotFound();
});

// Deposita um valor na conta identificada por id.
app.MapPost("/contas/{id:guid}/depositar", (Guid id, DepositoRequest request, IContaBancariaRepositorio repositorio) =>
{
    var conta = repositorio.ObterPorId(id);
    if (conta is null) return Results.NotFound();

    try
    {
        conta.Depositar(request.Valor);
        return Results.Ok(conta);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { erro = ex.Message });
    }
});

// Saca um valor da conta identificada por id.
// Trata exceções de domínio retornando 400, em vez de deixar a exceção estourar como 500.
app.MapPost("/contas/{id:guid}/sacar", (Guid id, SaqueRequest request, IContaBancariaRepositorio repositorio) =>
{
    var conta = repositorio.ObterPorId(id);
    if (conta is null) return Results.NotFound();

    try
    {
        conta.Sacar(request.Valor);
        return Results.Ok(conta);
    }
    catch (Exception ex) when (ex is ArgumentException or SaldoInsuficienteException)
    {
        return Results.BadRequest(new { erro = ex.Message });
    }
});

// Retorna o histórico de transações (depósitos e saques) da conta.
app.MapGet("/contas/{id:guid}/transacoes", (Guid id, IContaBancariaRepositorio repositorio) =>
{
    var conta = repositorio.ObterPorId(id);
    if (conta is null) return Results.NotFound();

    return Results.Ok(conta.Transacoes);
});

app.Run();

// DTO de entrada do POST /contas.
// Não expõe Id nem Saldo: o cliente não deve poder definir esses valores manualmente.
public record CriarContaRequest(string Titular);
public record DepositoRequest(decimal Valor);
public record SaqueRequest(decimal Valor);