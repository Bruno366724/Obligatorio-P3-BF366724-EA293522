using Dominio.Enumerados;
using Dominio.Excepciones;
using Dominio.InterfacesDominio;

namespace Dominio.Entidades;

public class Historia : IValidable
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Sinopsis { get; set; } = string.Empty;
    public EstadoHistoria Estado { get; set; } = EstadoHistoria.Borrador;
    public List<Categoria> Categorias { get; set; } = new();
    public List<Capitulo> Capitulos { get; set; } = new();
    public int? CapituloInicialId { get; set; }
    public CapituloIntermedio? CapituloInicial { get; set; }

    public void Validar()
    {
        if (string.IsNullOrWhiteSpace(Titulo))
            throw new HistoriaException("El título de la historia es obligatorio.");

        if (string.IsNullOrWhiteSpace(Sinopsis))
            throw new HistoriaException("La sinopsis es obligatoria.");

        if (Categorias.Count == 0)
            throw new HistoriaException("La historia debe pertenecer al menos a una categoría.");

        if (CapituloInicial is null)
            throw new HistoriaException("La historia debe tener un capítulo inicial.");
    }

    public void ValidarParaPublicar()
    {
        Validar();

        Capitulos.ForEach(capitulo => capitulo.Validar());

        Estado = EstadoHistoria.Publicada;
    }
}
