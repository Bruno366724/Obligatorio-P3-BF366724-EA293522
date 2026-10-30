using DTOs.DTOs;

namespace PresentacionUsuario.Models;

// Junta los nombres legibles (historia, final) para la pantalla de historial.
public class HistorialItemViewModel
{
    public required LecturaDTO Lectura { get; set; }
    public required string HistoriaTitulo { get; set; }
    public string? FinalTitulo { get; set; }
}
