using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios;

public interface IIniciarSesion
{
    UsuarioDTO Ejecutar(string nombreUsuario, string password);
}
