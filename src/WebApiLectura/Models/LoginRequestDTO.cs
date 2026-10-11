namespace WebApiLectura.Models;

// Body de POST /api/Autenticacion/Login.
public class LoginRequestDTO
{
    public string NombreUsuario { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
