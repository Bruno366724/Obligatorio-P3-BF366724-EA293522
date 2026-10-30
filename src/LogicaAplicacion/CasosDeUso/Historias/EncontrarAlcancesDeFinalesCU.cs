using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfacesRepositorios;
using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Historias;

namespace LogicaAplicacion.CasosDeUso.Historias;

// Finales de una historia con su cantidad de alcances, marcando el mas frecuente.
public class EncontrarAlcancesDeFinalesCU : IEncontrarAlcancesDeFinales
{
    private readonly IRepositorioHistoria repositorio;

    public EncontrarAlcancesDeFinalesCU(IRepositorioHistoria repo)
    {
        repositorio = repo;
    }

    public List<AlcanceFinalDTO> Ejecutar(int historiaId)
    {
        Historia historia = repositorio.FindByID(historiaId);
        if (historia is null)
            throw new HistoriaException("No se encontró la historia solicitada.");

        List<CapituloFinal> finales = historia.Capitulos.OfType<CapituloFinal>().ToList();
        int maximo = finales.Count == 0 ? 0 : finales.Max(final => final.ContadorAlcances);

        return finales
            .Select(final => new AlcanceFinalDTO
            {
                CapituloId = final.Id,
                Titulo = final.Titulo,
                ContadorAlcances = final.ContadorAlcances,
                EsMasFrecuente = maximo > 0 && final.ContadorAlcances == maximo
            })
            .OrderByDescending(fila => fila.ContadorAlcances)
            .ThenBy(fila => fila.Titulo)
            .ToList();
    }
}
