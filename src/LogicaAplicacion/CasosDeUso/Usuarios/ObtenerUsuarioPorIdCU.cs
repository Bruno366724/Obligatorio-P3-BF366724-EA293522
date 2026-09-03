using Dominio.InterfacesRepositorios;
using DTOs.DTOs;
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
        throw new NotImplementedException();
    }
}
