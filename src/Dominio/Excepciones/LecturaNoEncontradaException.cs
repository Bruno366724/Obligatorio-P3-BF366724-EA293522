namespace Dominio.Excepciones;

// La lectura pedida no existe.
public class LecturaNoEncontradaException : LecturaException
{
    public LecturaNoEncontradaException() { }

    public LecturaNoEncontradaException(string message) : base(message) { }

    public LecturaNoEncontradaException(string? message, Exception? innerException) : base(message, innerException) { }
}
