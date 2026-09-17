using Dominio.Excepciones;
using Dominio.InterfacesDominio;

namespace Dominio.Entidades;

public abstract class Capitulo : IValidable
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Texto { get; set; } = string.Empty;
    public int HistoriaId { get; set; }

    public virtual void Validar()
    {
        if (string.IsNullOrWhiteSpace(Titulo))
            throw new HistoriaException("El título del capítulo es obligatorio.");

        if (string.IsNullOrWhiteSpace(Texto))
            throw new HistoriaException("El texto del capítulo es obligatorio.");
    }
}
