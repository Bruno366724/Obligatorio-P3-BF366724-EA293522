using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas;

public interface IEncontrarHistorialLecturas
{
    List<LecturaDTO> Ejecutar(int usuarioId, int mes, int anio);
}
