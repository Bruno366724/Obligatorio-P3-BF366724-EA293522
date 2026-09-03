namespace DTOs.DTOs;

public class LecturaDTO
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public int HistoriaId { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public int? FinalAlcanzadoId { get; set; }
}
