using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Auditorias;

public interface IEncontrarAuditoriasPorAdministrador
{
    List<AuditoriaDTO> Ejecutar(int administradorId);
}
