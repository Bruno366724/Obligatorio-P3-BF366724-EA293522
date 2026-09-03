using Dominio.InterfacesRepositorios;
using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Historias;

namespace LogicaAplicacion.CasosDeUso.Historias;

public class ObtenerHistoriaPorIdCU : IObtenerHistoriaPorId
{
    private readonly IRepositorioHistoria repositorio;

    public ObtenerHistoriaPorIdCU(IRepositorioHistoria repo)
    {
        repositorio = repo;
    }

    public HistoriaDTO Ejecutar(int id)
    {
        throw new NotImplementedException();
    }
}
