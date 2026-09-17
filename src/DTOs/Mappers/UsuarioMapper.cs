using Dominio.Entidades;
using Dominio.ValueObjects;
using DTOs.DTOs;

namespace DTOs.Mappers;

public static class UsuarioMapper
{
    public static Usuario FromDTO(UsuarioDTO dto)
    {
        return new Usuario
        {
            Id = dto.Id,
            NombreCompleto = dto.NombreCompleto,
            Email = new Email(dto.Email),
            NombreUsuario = dto.NombreUsuario,
            Password = new Password(dto.Password),
            Rol = dto.Rol
        };
    }

    public static UsuarioDTO ToDTO(Usuario usuario)
    {
        return new UsuarioDTO
        {
            Id = usuario.Id,
            NombreCompleto = usuario.NombreCompleto,
            Email = usuario.Email.Direccion,
            NombreUsuario = usuario.NombreUsuario,
            Password = usuario.Password.Valor,
            Rol = usuario.Rol
        };
    }
}
