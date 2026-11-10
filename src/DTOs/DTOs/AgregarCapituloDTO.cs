namespace DTOs.DTOs;

// Datos para agregar un capitulo a continuacion de un intermedio (RF04).
public class AgregarCapituloDTO
{
    public int HistoriaId { get; set; }
    public int CapituloAntecedenteId { get; set; }
    public bool EsFinal { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Texto { get; set; } = string.Empty;
    public string TextoOpcion { get; set; } = string.Empty;
}
