using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios;

public interface IEncontrarTodosUsuarios
{
    List<UsuarioDTO> Ejecutar();
}
