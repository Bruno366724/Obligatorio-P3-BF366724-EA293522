using Dominio.InterfacesRepositorios;
using DTOs.DTOs;
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
        throw new NotImplementedException();
    }
}
