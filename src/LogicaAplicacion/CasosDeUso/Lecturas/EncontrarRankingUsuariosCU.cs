using Dominio.Entidades;
using Dominio.InterfacesRepositorios;
using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas;

namespace LogicaAplicacion.CasosDeUso.Lecturas;

// Usuarios ordenados por cantidad de historias distintas completadas, de mayor a menor.
public class EncontrarRankingUsuariosCU : IEncontrarRankingUsuarios
{
    private readonly IRepositorioLectura repositorio;

    public EncontrarRankingUsuariosCU(IRepositorioLectura repo)
    {
        repositorio = repo;
    }

    public List<RankingUsuarioDTO> Ejecutar()
    {
        IEnumerable<Lectura> completadas = repositorio.FindCompletadas();

        return completadas
            .GroupBy(lectura => lectura.UsuarioId)
            .Select(grupo => new RankingUsuarioDTO
            {
                UsuarioId = grupo.Key,
                NombreUsuario = grupo.First().Usuario!.NombreUsuario,
                NombreCompleto = grupo.First().Usuario!.NombreCompleto,
                CantidadHistorias = grupo.Select(lectura => lectura.HistoriaId).Distinct().Count()
            })
            .OrderByDescending(fila => fila.CantidadHistorias)
            .ThenBy(fila => fila.NombreUsuario)
            .ToList();
    }
}
