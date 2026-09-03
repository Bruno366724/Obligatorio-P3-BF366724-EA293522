using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Historias;

public interface IObtenerHistoriaPorId
{
    HistoriaDTO Ejecutar(int id);
}
