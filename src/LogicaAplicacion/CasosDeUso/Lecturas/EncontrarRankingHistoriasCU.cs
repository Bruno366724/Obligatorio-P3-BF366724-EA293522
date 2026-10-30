using Dominio.Entidades;
using Dominio.InterfacesRepositorios;
using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas;

namespace LogicaAplicacion.CasosDeUso.Lecturas;

// Historias ordenadas por cantidad de lecturas completadas, de mayor a menor.
public class EncontrarRankingHistoriasCU : IEncontrarRankingHistorias
{
    private readonly IRepositorioLectura repositorio;

    public EncontrarRankingHistoriasCU(IRepositorioLectura repo)
    {
        repositorio = repo;
    }

    public List<RankingHistoriaDTO> Ejecutar()
    {
        IEnumerable<Lectura> completadas = repositorio.FindCompletadas();

        return completadas
            .GroupBy(lectura => lectura.HistoriaId)
            .Select(grupo => new RankingHistoriaDTO
            {
                HistoriaId = grupo.Key,
                Titulo = grupo.First().Historia!.Titulo,
                CantidadLecturas = grupo.Count()
            })
            .OrderByDescending(fila => fila.CantidadLecturas)
            .ThenBy(fila => fila.Titulo)
            .ToList();
    }
}
