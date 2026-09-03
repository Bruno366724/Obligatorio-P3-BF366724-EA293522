using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas;

public interface IEncontrarTodasLecturas
{
    List<LecturaDTO> Ejecutar();
}
