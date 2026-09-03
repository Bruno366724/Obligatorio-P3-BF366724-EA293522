using Dominio.Enumerados;

namespace DTOs.DTOs;

public class HistoriaDTO
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Sinopsis { get; set; } = string.Empty;
    public EstadoHistoria Estado { get; set; }
    public List<CategoriaDTO> Categorias { get; set; } = new();
    public List<CapituloDTO> Capitulos { get; set; } = new();
    public int CapituloInicialId { get; set; }
}
