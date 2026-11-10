namespace Dominio.Excepciones;

// La historia pedida no existe.
public class HistoriaNoEncontradaException : HistoriaException
{
    public HistoriaNoEncontradaException() { }

    public HistoriaNoEncontradaException(string message) : base(message) { }

    public HistoriaNoEncontradaException(string? message, Exception? innerException) : base(message, innerException) { }
}
