namespace PlataformaNexbank.Domain.Exceptions;

// Base para toda violação de regra de negócio do domínio.
// Permite que a Api trate todas de forma uniforme (422) sem conhecer cada tipo.
public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message) { }
}