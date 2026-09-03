namespace DTOs.DTOs;

public class AuditoriaDTO
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public string Accion { get; set; } = string.Empty;
    public int AdministradorId { get; set; }
    public int HistoriaId { get; set; }
    public int? CapituloId { get; set; }
}
