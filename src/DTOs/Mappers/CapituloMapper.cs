using Dominio.Entidades;
using Dominio.Excepciones;
using DTOs.DTOs;

namespace DTOs.Mappers;

public static class CapituloMapper
{
    public static Capitulo FromDTO(CapituloDTO dto)
    {
        return dto switch
        {
            CapituloIntermedioDTO intermedio => new CapituloIntermedio
            {
                Id = intermedio.Id,
                Titulo = intermedio.Titulo,
                Texto = intermedio.Texto,
                HistoriaId = intermedio.HistoriaId,
                Opciones = intermedio.Opciones.Select(OpcionMapper.FromDTO).ToList()
            },
            CapituloFinalDTO final => new CapituloFinal
            {
                Id = final.Id,
                Titulo = final.Titulo,
                Texto = final.Texto,
                HistoriaId = final.HistoriaId,
                ContadorAlcances = final.ContadorAlcances
            },
            _ => throw new HistoriaException("Tipo de capítulo desconocido.")
        };
    }

    public static CapituloDTO ToDTO(Capitulo capitulo)
    {
        return capitulo switch
        {
            CapituloIntermedio intermedio => new CapituloIntermedioDTO
            {
                Id = intermedio.Id,
                Titulo = intermedio.Titulo,
                Texto = intermedio.Texto,
                HistoriaId = intermedio.HistoriaId,
                Opciones = intermedio.Opciones
                    .Select(opcion =>
                    {
                        OpcionDTO opcionDto = OpcionMapper.ToDTO(opcion);
                        opcionDto.CapituloIntermedioId = intermedio.Id;
                        return opcionDto;
                    })
                    .ToList()
            },
            CapituloFinal final => new CapituloFinalDTO
            {
                Id = final.Id,
                Titulo = final.Titulo,
                Texto = final.Texto,
                HistoriaId = final.HistoriaId,
                ContadorAlcances = final.ContadorAlcances
            },
            _ => throw new HistoriaException("Tipo de capítulo desconocido.")
        };
    }
}
