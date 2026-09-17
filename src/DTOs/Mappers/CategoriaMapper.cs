using Dominio.Entidades;
using DTOs.DTOs;

namespace DTOs.Mappers;

public static class CategoriaMapper
{
    public static Categoria FromDTO(CategoriaDTO dto)
    {
        return new Categoria
        {
            Id = dto.Id,
            Nombre = dto.Nombre
        };
    }

    public static CategoriaDTO ToDTO(Categoria categoria)
    {
        return new CategoriaDTO
        {
            Id = categoria.Id,
            Nombre = categoria.Nombre
        };
    }
}
