using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios;

public interface IObtenerUsuarioPorId
{
    UsuarioDTO Ejecutar(int id);
}
