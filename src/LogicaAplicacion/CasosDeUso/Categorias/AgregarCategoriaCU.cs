using Dominio.InterfacesRepositorios;
using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Categorias;

namespace LogicaAplicacion.CasosDeUso.Categorias;

public class AgregarCategoriaCU : IAgregarCategoria
{
    private readonly IRepositorioCategoria repositorio;

    public AgregarCategoriaCU(IRepositorioCategoria repo)
    {
        repositorio = repo;
    }

    public void Ejecutar(CategoriaDTO dto)
    {
        throw new NotImplementedException();
    }
}
