using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Historias;

public interface IEncontrarTodasHistorias
{
    List<HistoriaDTO> Ejecutar();
}
