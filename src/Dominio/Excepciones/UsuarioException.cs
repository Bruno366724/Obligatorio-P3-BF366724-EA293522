namespace Dominio.Excepciones;

public class UsuarioException : DomainException
{
    public UsuarioException()
    {
    }

    public UsuarioException(string message) : base(message)
    {
    }

    public UsuarioException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
