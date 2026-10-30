using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Historias;

public interface IEncontrarAlcancesDeFinales
{
    List<AlcanceFinalDTO> Ejecutar(int historiaId);
}
