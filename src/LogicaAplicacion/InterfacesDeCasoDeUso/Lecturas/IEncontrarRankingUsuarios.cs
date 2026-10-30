using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas;

public interface IEncontrarRankingUsuarios
{
    List<RankingUsuarioDTO> Ejecutar();
}
