using Dominio.InterfacesRepositorios;
using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas;

namespace LogicaAplicacion.CasosDeUso.Lecturas;

public class ObtenerLecturaPorIdCU : IObtenerLecturaPorId
{
    private readonly IRepositorioLectura repositorio;

    public ObtenerLecturaPorIdCU(IRepositorioLectura repo)
    {
        repositorio = repo;
    }

    public LecturaDTO Ejecutar(int id)
    {
        throw new NotImplementedException();
    }
}
