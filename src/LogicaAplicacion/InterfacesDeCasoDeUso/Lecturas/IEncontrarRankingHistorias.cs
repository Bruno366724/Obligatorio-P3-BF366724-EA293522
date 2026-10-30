using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas;

public interface IEncontrarRankingHistorias
{
    List<RankingHistoriaDTO> Ejecutar();
}
