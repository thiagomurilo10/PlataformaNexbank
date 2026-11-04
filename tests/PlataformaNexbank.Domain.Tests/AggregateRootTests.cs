using PlataformaNexbank.Domain.Events;

namespace PlataformaNexbank.Domain.Tests;

public class AggregateRootTests
{
    private sealed record EventoDeTeste : DomainEvent;

    private sealed class AgregadoDeTeste : AggregateRoot<Guid>
    {
        public AgregadoDeTeste() : base(Guid.NewGuid())
        {
        }

        public void Registrar(IDomainEvent evento) => AdicionarEvento(evento);
    }

    [Fact]
    public void NovoAgregado_NaoPossuiEventos()
    {
        var agregado = new AgregadoDeTeste();

        Assert.Empty(agregado.DomainEvents);
    }

    [Fact]
    public void AdicionarEvento_AcumulaEventosNaOrdemDeRegistro()
    {
        var agregado = new AgregadoDeTeste();
        var primeiro = new EventoDeTeste();
        var segundo = new EventoDeTeste();

        agregado.Registrar(primeiro);
        agregado.Registrar(segundo);

        Assert.Equal(new IDomainEvent[] { primeiro, segundo }, agregado.DomainEvents);
    }

    [Fact]
    public void LimparEventos_EsvaziaAColecao()
    {
        var agregado = new AgregadoDeTeste();
        agregado.Registrar(new EventoDeTeste());

        agregado.LimparEventos();

        Assert.Empty(agregado.DomainEvents);
    }

    [Fact]
    public void AdicionarEvento_Nulo_LancaArgumentNullException()
    {
        var agregado = new AgregadoDeTeste();

        Assert.Throws<ArgumentNullException>(() => agregado.Registrar(null!));
    }
}