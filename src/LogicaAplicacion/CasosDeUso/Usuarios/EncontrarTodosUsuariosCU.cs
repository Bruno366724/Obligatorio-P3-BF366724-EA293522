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
        List<Usuario> usuarios = (List<Usuario>)repositorio.FindAll();
        List<UsuarioDTO> aRetornar = new List<UsuarioDTO>();
        foreach (Usuario usuario in usuarios)
        {
            aRetornar.Add(UsuarioMapper.ToDTO(usuario));
        }
        return aRetornar;
    }
}
