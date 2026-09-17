namespace Dominio.Excepciones;

public class LecturaException : DomainException
{
    public LecturaException()
    {
    }

    public LecturaException(string message) : base(message)
    {
    }

    public LecturaException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
