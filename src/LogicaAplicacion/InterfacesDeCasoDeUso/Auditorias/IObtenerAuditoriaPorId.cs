using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Auditorias;

public interface IObtenerAuditoriaPorId
{
    AuditoriaDTO Ejecutar(int id);
}
