namespace Dominio.Excepciones;

// La lectura pertenece a otro usuario.
public class LecturaAjenaException : LecturaException
{
    public LecturaAjenaException() { }

    public LecturaAjenaException(string message) : base(message) { }

    public LecturaAjenaException(string? message, Exception? innerException) : base(message, innerException) { }
}
