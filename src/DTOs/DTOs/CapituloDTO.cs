namespace DTOs.DTOs;

public abstract class CapituloDTO
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Texto { get; set; } = string.Empty;
    public int HistoriaId { get; set; }
}
