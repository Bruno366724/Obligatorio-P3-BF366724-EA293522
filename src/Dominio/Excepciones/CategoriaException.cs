namespace Dominio.Excepciones;

public class CategoriaException : DomainException
{
    public CategoriaException()
    {
    }

    public CategoriaException(string message) : base(message)
    {
    }

    public CategoriaException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
