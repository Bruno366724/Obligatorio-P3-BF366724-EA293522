using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfacesRepositorios;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios;

namespace LogicaAplicacion.CasosDeUso.Usuarios;

public class AgregarUsuarioCU : IAgregarUsuario
{
    private readonly IRepositorioUsuario repositorio;

    public AgregarUsuarioCU(IRepositorioUsuario repo)
    {
        repositorio = repo;
    }

    public void Ejecutar(UsuarioDTO dto)
    {
        if (repositorio.FindByNombreUsuario(dto.NombreUsuario) is not null)
            throw new UsuarioException("Ya existe un usuario con ese nombre de usuario.");

        Usuario usuario = UsuarioMapper.FromDTO(dto);
        usuario.Validar();
        repositorio.Add(usuario);
    }
}
