using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Auditorias;

public interface IAgregarAuditoria
{
    void Ejecutar(AuditoriaDTO dto);
}
