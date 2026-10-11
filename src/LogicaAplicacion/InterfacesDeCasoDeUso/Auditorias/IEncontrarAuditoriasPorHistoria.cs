using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Auditorias;

public interface IEncontrarAuditoriasPorHistoria
{
    List<AuditoriaDTO> Ejecutar(int historiaId);
}
