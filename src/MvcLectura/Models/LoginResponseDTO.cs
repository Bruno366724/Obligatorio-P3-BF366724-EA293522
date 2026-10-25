using DTOs.DTOs;

namespace MvcLectura.Models;

// Forma de lo que devuelve POST /api/Auth/login en WebApiLectura.
public class LoginResponseDTO
{
    public string Token { get; set; } = string.Empty;
    public UsuarioDTO Usuario { get; set; } = new UsuarioDTO();
}
