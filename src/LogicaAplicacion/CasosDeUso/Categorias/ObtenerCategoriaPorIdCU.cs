using Dominio.InterfacesRepositorios;
using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Categorias;

namespace LogicaAplicacion.CasosDeUso.Categorias;

public class ObtenerCategoriaPorIdCU : IObtenerCategoriaPorId
{
    private readonly IRepositorioCategoria repositorio;

    public ObtenerCategoriaPorIdCU(IRepositorioCategoria repo)
    {
        repositorio = repo;
    }

    public CategoriaDTO Ejecutar(int id)
    {
        throw new NotImplementedException();
    }
}
