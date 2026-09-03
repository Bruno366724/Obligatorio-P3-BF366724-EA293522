namespace DTOs.DTOs;

public class OpcionDTO
{
    public int Id { get; set; }
    public string Texto { get; set; } = string.Empty;
    public int CapituloIntermedioId { get; set; }
    public int CapituloDestinoId { get; set; }
}
