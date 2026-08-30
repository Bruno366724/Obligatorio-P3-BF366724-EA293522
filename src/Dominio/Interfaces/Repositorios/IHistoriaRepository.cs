using Dominio.Entidades;
using Dominio.Enumerados;

namespace Dominio.Interfaces.Repositorios;

public interface IHistoriaRepository : IRepositorio<Historia>
{
    IEnumerable<Historia> ObtenerPorEstado(EstadoHistoria estado);
    IEnumerable<Historia> ObtenerPorCategoria(int categoriaId);
}
