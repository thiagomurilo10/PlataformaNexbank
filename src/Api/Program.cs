using Microsoft.EntityFrameworkCore;
using PlataformaNexbank.Api;
using PlataformaNexbank.Application.DTOs;
using PlataformaNexbank.Application.UseCases;
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

// Use cases registrados como Scoped, alinhados ao ciclo de vida do repositório/DbContext.
builder.Services.AddScoped<CriarContaUseCase>();
builder.Services.AddScoped<ObterContaUseCase>();
builder.Services.AddScoped<DepositarUseCase>();
builder.Services.AddScoped<SacarUseCase>();
builder.Services.AddScoped<ListarTransacoesUseCase>();

var connectionString = builder.Configuration.GetConnectionString("NexBankDb");

builder.Services.AddDbContext<NexBankDbContext>(options =>
    options.UseNpgsql(connectionString));

// Registra o handler de exceções global (ApiExceptionHandler) e habilita o formato padrão de erro ProblemDetails (RFC 7807) nas respostas.
builder.Services.AddExceptionHandler<ApiExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

//Qualquer exceção não tratada nos endpoints passa a ser capturada aqui pelo ApiExceptionHandler, em vez de precisar de try/catch manual em cada rota.
app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Cria uma conta nova. O endpoint não conhece mais o domínio diretamente — só repassa o DTO de request ao use case e devolve o DTO de response.
app.MapPost("/contas", (CriarContaRequest request, CriarContaUseCase useCase) =>
{
    var response = useCase.Executar(request);
    return Results.Created($"/contas/{response.Id}", response);
});

// Busca uma conta pelo Id. {id:guid} rejeita valores que não sejam Guid válido antes de chegar aqui.
app.MapGet("/contas/{id:guid}", (Guid id, ObterContaUseCase useCase) =>
{
    var response = useCase.Executar(id);
    return response is not null ? Results.Ok(response) : Results.NotFound();
});

// Deposita um valor na conta
app.MapPost("/contas/{id:guid}/depositar", (Guid id, DepositarRequest request, DepositarUseCase useCase) =>
{
    var response = useCase.Executar(id, request);
    return response is not null ? Results.Ok(response) : Results.NotFound();
});

// Saca um valor da conta. Trata tanto validação de valor quanto regra de saldo insuficiente, ambas lançadas pelo domínio e propagadas sem tratamento pelo use case.
app.MapPost("/contas/{id:guid}/sacar", (Guid id, SacarRequest request, SacarUseCase useCase) =>
{
    var response = useCase.Executar(id, request);
    return response is not null ? Results.Ok(response) : Results.NotFound();
});

// Retorna o histórico de transações da conta, já convertido para DTO
app.MapGet("/contas/{id:guid}/transacoes", (Guid id, ListarTransacoesUseCase useCase) =>
{
    var response = useCase.Executar(id);
    return response is not null ? Results.Ok(response) : Results.NotFound();
});

app.Run();
