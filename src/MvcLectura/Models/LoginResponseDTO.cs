using Dominio.Enumerados;

namespace MvcLectura.Models;

// Forma de lo que devuelve POST /api/Autenticacion/Login en WebApiLectura.
public class LoginResponseDTO
{
    public string Token { get; set; } = string.Empty;
    public int Id { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public RolUsuario Rol { get; set; }
}
