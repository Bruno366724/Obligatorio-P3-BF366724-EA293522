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
        // No está explícito en la letra, pero un nombre de usuario repetido
        // rompería el login (RF01), que identifica al usuario por este campo.
        if (repositorio.FindByNombreUsuario(dto.NombreUsuario) is not null)
            throw new DomainException("Ya existe un usuario con ese nombre de usuario.");

        var usuario = UsuarioMapper.FromDTO(dto);
        usuario.Validar();
        repositorio.Add(usuario);
    }
}
