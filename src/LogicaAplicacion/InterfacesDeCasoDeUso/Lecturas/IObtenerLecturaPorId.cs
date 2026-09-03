using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas;

public interface IObtenerLecturaPorId
{
    LecturaDTO Ejecutar(int id);
}
