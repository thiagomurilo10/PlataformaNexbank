using PlataformaNexbank.Domain.ValueObjects;

namespace PlataformaNexbank.Domain.Tests;

// Titular válido reutilizável. Seguro como campo estático porque o VO é imutável.
internal static class TitularBuilder
{
    public static readonly Titular Padrao =
        Titular.Criar("Thiago Murilo", Cpf.Criar("52998224725"));
}