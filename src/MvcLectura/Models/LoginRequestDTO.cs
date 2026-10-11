namespace MvcLectura.Models;

// Lo que espera el body de POST /api/Autenticacion/Login en WebApiLectura.
public class LoginRequestDTO
{
    public string NombreUsuario { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
