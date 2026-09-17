using Dominio.Excepciones;
using Dominio.InterfacesDominio;

namespace Dominio.Entidades;

public class Lectura : IValidable
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }
    public int HistoriaId { get; set; }
    public Historia? Historia { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public int? FinalAlcanzadoId { get; set; }
    public CapituloFinal? FinalAlcanzado { get; set; }

    public void Validar()
    {
        if (UsuarioId <= 0 && Usuario is null)
            throw new LecturaException("La lectura debe estar asociada a un usuario.");

        if (HistoriaId <= 0 && Historia is null)
            throw new LecturaException("La lectura debe estar asociada a una historia.");

        if (FechaFin.HasValue && FechaFin.Value < FechaInicio)
            throw new LecturaException("La fecha de finalización no puede ser anterior a la fecha de inicio.");
    }
}
