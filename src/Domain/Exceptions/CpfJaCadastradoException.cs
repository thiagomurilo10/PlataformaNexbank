namespace PlataformaNexbank.Domain.Exceptions;

// Regra de unicidade: uma conta por CPF. A mensagem não repete o CPF, para não expor dado pessoal em resposta de erro.
public class CpfJaCadastradoException : Exception
{
    public CpfJaCadastradoException()
        : base("Já existe uma conta cadastrada para este CPF.")
    {
    }
}