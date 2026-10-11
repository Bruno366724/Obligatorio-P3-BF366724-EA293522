using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Historias;

public interface IEncontrarHistoriasPublicadas
{
    List<HistoriaDTO> Ejecutar(int? categoriaId);
}
