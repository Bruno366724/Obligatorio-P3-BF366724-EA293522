using Dominio.Entidades;
using Dominio.Excepciones;
using DTOs.DTOs;

namespace DTOs.Mappers;

public static class HistoriaMapper
{
    public static Historia FromDTO(HistoriaDTO dto)
    {
        List<Capitulo> capitulos = dto.Capitulos.Select(CapituloMapper.FromDTO).ToList();

        CapituloIntermedio capituloInicial = capitulos.FirstOrDefault(c => c.Id == dto.CapituloInicialId) as CapituloIntermedio
            ?? throw new HistoriaException("No se encontró el capítulo inicial indicado entre los capítulos de la historia.");

        return new Historia
        {
            Id = dto.Id,
            Titulo = dto.Titulo,
            Sinopsis = dto.Sinopsis,
            Estado = dto.Estado,
            Categorias = dto.Categorias.Select(CategoriaMapper.FromDTO).ToList(),
            Capitulos = capitulos,
            CapituloInicialId = dto.CapituloInicialId,
            CapituloInicial = capituloInicial
        };
    }

    public static HistoriaDTO ToDTO(Historia historia)
    {
        return new HistoriaDTO
        {
            Id = historia.Id,
            Titulo = historia.Titulo,
            Sinopsis = historia.Sinopsis,
            Estado = historia.Estado,
            Categorias = historia.Categorias.Select(CategoriaMapper.ToDTO).ToList(),
            Capitulos = historia.Capitulos.Select(CapituloMapper.ToDTO).ToList(),
            CapituloInicialId = historia.CapituloInicialId
        };
    }
}
