using DTOs.DTOs;

namespace PresentacionUsuario.Models;

// Junta la Lectura, la Historia y el capitulo actual para la vista.
public class LeerViewModel
{
    public required LecturaDTO Lectura { get; set; }
    public required HistoriaDTO Historia { get; set; }
    public required CapituloDTO CapituloActual { get; set; }
}
