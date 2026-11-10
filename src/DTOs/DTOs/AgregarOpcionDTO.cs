namespace DTOs.DTOs;

// Datos para enlazar un capitulo intermedio con otro ya existente de la misma historia.
public class AgregarOpcionDTO
{
    public int HistoriaId { get; set; }
    public int CapituloOrigenId { get; set; }
    public int CapituloDestinoId { get; set; }
    public string TextoOpcion { get; set; } = string.Empty;
}
