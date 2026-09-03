using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios;

public interface IAgregarUsuario
{
    void Ejecutar(UsuarioDTO dto);
}
