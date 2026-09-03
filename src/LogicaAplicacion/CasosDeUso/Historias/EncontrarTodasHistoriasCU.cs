using Dominio.InterfacesRepositorios;
using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Historias;

namespace LogicaAplicacion.CasosDeUso.Historias;

public class EncontrarTodasHistoriasCU : IEncontrarTodasHistorias
{
    private readonly IRepositorioHistoria repositorio;

    public EncontrarTodasHistoriasCU(IRepositorioHistoria repo)
    {
        repositorio = repo;
    }

    public List<HistoriaDTO> Ejecutar()
    {
        throw new NotImplementedException();
    }
}
