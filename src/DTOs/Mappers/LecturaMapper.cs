using Dominio.Entidades;
using DTOs.DTOs;

namespace DTOs.Mappers;

public static class LecturaMapper
{
    public static Lectura FromDTO(LecturaDTO dto)
    {
        return new Lectura
        {
            Id = dto.Id,
            UsuarioId = dto.UsuarioId,
            HistoriaId = dto.HistoriaId,
            CapituloActualId = dto.CapituloActualId,
            FechaInicio = dto.FechaInicio,
            FechaFin = dto.FechaFin,
            FinalAlcanzadoId = dto.FinalAlcanzadoId
        };
    }

    public static LecturaDTO ToDTO(Lectura lectura)
    {
        return new LecturaDTO
        {
            Id = lectura.Id,
            UsuarioId = lectura.UsuarioId,
            HistoriaId = lectura.HistoriaId,
            CapituloActualId = lectura.CapituloActualId,
            FechaInicio = lectura.FechaInicio,
            FechaFin = lectura.FechaFin,
            FinalAlcanzadoId = lectura.FinalAlcanzadoId
        };
    }
}
