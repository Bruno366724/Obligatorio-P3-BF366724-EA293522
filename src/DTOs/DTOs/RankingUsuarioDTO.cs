namespace DTOs.DTOs;

// Una fila del ranking de usuarios por historias distintas completadas.
public class RankingUsuarioDTO
{
    public int UsuarioId { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public int CantidadHistorias { get; set; }
}
