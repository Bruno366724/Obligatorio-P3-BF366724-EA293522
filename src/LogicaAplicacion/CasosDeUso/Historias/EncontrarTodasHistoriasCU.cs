using Dominio.InterfacesRepositorios;
using DTOs.DTOs;
using DTOs.Mappers;
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
        return repositorio.FindAll()
            .Select(historia => HistoriaMapper.ToDTO(historia))
            .ToList();
    }
}
