using PlataformaNexbank.Domain.ValueObjects;

namespace PlataformaNexbank.Domain.Tests;

public class EntityTests
{
    private sealed class EntidadeA : Entity<ContaId>
    {
        public EntidadeA(ContaId id) : base(id) { }
    }

    private sealed class EntidadeB : Entity<ContaId>
    {
        public EntidadeB(ContaId id) : base(id) { }
    }

    [Fact]
    public void Equals_ComMesmoIdEMesmoTipo_DeveSerIgual()
    {
        var id = ContaId.Novo();

        var a = new EntidadeA(id);
        var b = new EntidadeA(id);

        Assert.Equal(a, b);
        Assert.True(a == b);
        Assert.False(a != b);
    }

    [Fact]
    public void Equals_ComIdsDiferentes_DeveSerDiferente()
    {
        var a = new EntidadeA(ContaId.Novo());
        var b = new EntidadeA(ContaId.Novo());

        Assert.NotEqual(a, b);
        Assert.True(a != b);
    }

    [Fact]
    public void Equals_ComMesmoIdETiposDiferentes_DeveSerDiferente()
    {
        var id = ContaId.Novo();

        var a = new EntidadeA(id);
        var b = new EntidadeB(id);

        Assert.False(a.Equals(b));
    }

    [Fact]
    public void Equals_ComNulo_DeveSerFalso()
    {
        var a = new EntidadeA(ContaId.Novo());

        Assert.False(a.Equals(null));
        Assert.False(a == null);
        Assert.True(a != null);
    }

    [Fact]
    public void Equals_ComIdPadrao_DeveSerIgualApenasPorReferencia()
    {
        var a = new EntidadeA(default);
        var b = new EntidadeA(default);

        Assert.False(a == b);
        Assert.True(a.Equals(a));
    }

    [Fact]
    public void GetHashCode_ComMesmoId_DeveSerIgual()
    {
        var id = ContaId.Novo();

        var a = new EntidadeA(id);
        var b = new EntidadeA(id);

        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }
}