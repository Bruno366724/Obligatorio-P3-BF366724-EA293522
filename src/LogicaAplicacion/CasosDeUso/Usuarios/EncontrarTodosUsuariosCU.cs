using Dominio.Entidades;
using Dominio.InterfacesRepositorios;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios;

namespace LogicaAplicacion.CasosDeUso.Usuarios;

public class EncontrarTodosUsuariosCU : IEncontrarTodosUsuarios
{
    private readonly IRepositorioUsuario repositorio;

    public EncontrarTodosUsuariosCU(IRepositorioUsuario repo)
    {
        repositorio = repo;
    }

    public List<UsuarioDTO> Ejecutar()
    {
        return repositorio.FindAll()
            .Select(usuario => UsuarioMapper.ToDTO(usuario))
            .ToList();
    }
}
