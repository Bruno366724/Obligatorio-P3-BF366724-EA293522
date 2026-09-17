using Dominio.Entidades;
using DTOs.DTOs;

namespace DTOs.Mappers;

public static class OpcionMapper
{
    public static Opcion FromDTO(OpcionDTO dto)
    {
        return new Opcion
        {
            Id = dto.Id,
            Texto = dto.Texto,
            CapituloDestinoId = dto.CapituloDestinoId
        };
    }

    public static OpcionDTO ToDTO(Opcion opcion)
    {
        return new OpcionDTO
        {
            Id = opcion.Id,
            Texto = opcion.Texto,
            CapituloDestinoId = opcion.CapituloDestinoId
        };
    }
}
