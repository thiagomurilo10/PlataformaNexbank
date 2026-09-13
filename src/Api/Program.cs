using PlataformaNexbank.Domain.Entities;
using PlataformaNexbank.Domain.Repositories;
using PlataformaNexbank.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Registra o repositório in-memory como implementação da interface do Domain.
// Singleton: uma única instância viva durante toda a execução da aplicação,
// necessário para o Dictionary in-memory não perder dados entre requisições.
builder.Services.AddSingleton<IContaBancariaRepositorio, ContaBancariaRepositorioInMemory>();

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

app.Run();

// DTO de entrada do POST /contas.
// Não expõe Id nem Saldo: o cliente não deve poder definir esses valores manualmente.
public record CriarContaRequest(string Titular);