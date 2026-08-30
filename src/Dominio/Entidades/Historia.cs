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
    public int CapituloInicialId { get; set; }
    public Capitulo? CapituloInicial { get; set; }

    public void Validar()
    {
        if (string.IsNullOrWhiteSpace(Titulo))
            throw new DomainException("El título de la historia es obligatorio.");

        if (string.IsNullOrWhiteSpace(Sinopsis))
            throw new DomainException("La sinopsis es obligatoria.");

        if (Categorias.Count == 0)
            throw new DomainException("La historia debe pertenecer al menos a una categoría.");

        if (CapituloInicial is null)
            throw new DomainException("La historia debe tener un capítulo inicial.");

        if (CapituloInicial is CapituloFinal)
            throw new DomainException("El capítulo inicial no puede ser de tipo final.");
    }

    public void ValidarParaPublicar()
    {
        Validar();

        foreach (var capitulo in Capitulos)
            capitulo.Validar();

        Estado = EstadoHistoria.Publicada;
    }
}
