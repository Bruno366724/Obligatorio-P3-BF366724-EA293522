using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfacesRepositorios;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios;

namespace LogicaAplicacion.CasosDeUso.Usuarios;

// Login de la app de Lectura: valida credencial, sin restriccion de rol.
public class IniciarSesionLecturaCU : IIniciarSesionLectura
{
    private readonly IRepositorioUsuario repositorio;

    public IniciarSesionLecturaCU(IRepositorioUsuario repo)
    {
        repositorio = repo;
    }

    public UsuarioDTO Ejecutar(string nombreUsuario, string password)
    {
        Usuario usuario = repositorio.FindByNombreUsuario(nombreUsuario);
        if (usuario is null || usuario.Password.Valor != password)
            throw new UsuarioException("Usuario o contraseña incorrectos.");

        return UsuarioMapper.ToDTO(usuario);
    }
}
