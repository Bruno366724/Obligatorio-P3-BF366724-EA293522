using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios;

public interface IIniciarSesionLectura
{
    UsuarioDTO Ejecutar(string nombreUsuario, string password);
}
