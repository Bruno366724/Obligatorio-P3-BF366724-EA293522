namespace DTOs.DTOs;

// Una fila del ranking de historias mas leidas.
public class RankingHistoriaDTO
{
    public int HistoriaId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public int CantidadLecturas { get; set; }
}
