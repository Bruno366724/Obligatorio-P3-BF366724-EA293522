using Dominio.Entidades;
using Dominio.Enumerados;
using Dominio.Excepciones;
using Dominio.InterfacesRepositorios;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios;

namespace LogicaAplicacion.CasosDeUso.Usuarios;

public class IniciarSesionCU : IIniciarSesion
{
    private readonly IRepositorioUsuario repositorio;

    public IniciarSesionCU(IRepositorioUsuario repo)
    {
        repositorio = repo;
    }

    public UsuarioDTO Ejecutar(string nombreUsuario, string password)
    {
        Usuario usuario = repositorio.FindByNombreUsuario(nombreUsuario);
        if (usuario is null || usuario.Password.Valor != password)
            throw new UsuarioException("Usuario o contraseña incorrectos.");

        if (usuario.Rol != RolUsuario.Administrador)
            throw new UsuarioException("Solo los administradores pueden ingresar a esta aplicación.");

        return UsuarioMapper.ToDTO(usuario);
    }
}
