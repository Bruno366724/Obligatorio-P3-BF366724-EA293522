using Dominio.InterfacesRepositorios;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Categorias;

namespace LogicaAplicacion.CasosDeUso.Categorias;

public class EncontrarTodasCategoriasCU : IEncontrarTodasCategorias
{
    private readonly IRepositorioCategoria repositorio;

    public EncontrarTodasCategoriasCU(IRepositorioCategoria repo)
    {
        repositorio = repo;
    }

    public List<CategoriaDTO> Ejecutar()
    {
        return repositorio.FindAll()
            .Select(categoria => CategoriaMapper.ToDTO(categoria))
            .ToList();
    }
}
