using Dominio.Enumerados;
using Dominio.Excepciones;
using Dominio.InterfacesDominio;
using Dominio.ValueObjects;

namespace Dominio.Entidades;

public class Usuario : IValidable
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public Email Email { get; set; } = null!;
    public string NombreUsuario { get; set; } = string.Empty;
    public Password Password { get; set; } = null!;
    public RolUsuario Rol { get; set; }

    public void Validar()
    {
        if (string.IsNullOrWhiteSpace(NombreCompleto))
            throw new UsuarioException("El nombre completo es obligatorio.");

        if (string.IsNullOrWhiteSpace(NombreUsuario))
            throw new UsuarioException("El nombre de usuario es obligatorio.");

        if (Email is null)
            throw new UsuarioException("El email es obligatorio.");

        if (Password is null)
            throw new UsuarioException("La contraseña es obligatoria.");

        Email.Validar();
        Password.Validar();
    }
}
