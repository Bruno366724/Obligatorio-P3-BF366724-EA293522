using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfacesRepositorios;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios;

namespace LogicaAplicacion.CasosDeUso.Usuarios;

public class ObtenerUsuarioPorIdCU : IObtenerUsuarioPorId
{
    private readonly IRepositorioUsuario repositorio;

    public ObtenerUsuarioPorIdCU(IRepositorioUsuario repo)
    {
        repositorio = repo;
    }

    public UsuarioDTO Ejecutar(int id)
    {
        Usuario usuario = repositorio.FindByID(id);
        if (usuario is null)
            throw new UsuarioException("No se encontró el usuario solicitado.");

        return UsuarioMapper.ToDTO(usuario);
    }
}
