using Dominio.Entidades;
using DTOs.DTOs;

namespace DTOs.Mappers;

public static class AuditoriaMapper
{
    public static Auditoria FromDTO(AuditoriaDTO dto)
    {
        return new Auditoria
        {
            Id = dto.Id,
            Fecha = dto.Fecha,
            Accion = dto.Accion,
            AdministradorId = dto.AdministradorId,
            HistoriaId = dto.HistoriaId,
            CapituloId = dto.CapituloId
        };
    }

    public static AuditoriaDTO ToDTO(Auditoria auditoria)
    {
        return new AuditoriaDTO
        {
            Id = auditoria.Id,
            Fecha = auditoria.Fecha,
            Accion = auditoria.Accion,
            AdministradorId = auditoria.AdministradorId,
            HistoriaId = auditoria.HistoriaId,
            CapituloId = auditoria.CapituloId
        };
    }
}
