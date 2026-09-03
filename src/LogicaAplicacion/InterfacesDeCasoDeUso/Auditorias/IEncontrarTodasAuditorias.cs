using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Auditorias;

public interface IEncontrarTodasAuditorias
{
    List<AuditoriaDTO> Ejecutar();
}
