using DTOs.DTOs;

namespace PresentacionUsuario.Models;

// Combo de historias + finales de la historia elegida (si hay una elegida).
public class FinalesPorHistoriaViewModel
{
    public List<HistoriaDTO> Historias { get; set; } = [];
    public int? HistoriaId { get; set; }
    public List<AlcanceFinalDTO> Finales { get; set; } = [];
}
