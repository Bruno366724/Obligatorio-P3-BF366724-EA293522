namespace Dominio.Excepciones;

public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }
}
