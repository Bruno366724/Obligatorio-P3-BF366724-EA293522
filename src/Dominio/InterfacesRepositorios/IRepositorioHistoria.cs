using Dominio.Entidades;
using Dominio.Enumerados;

namespace Dominio.InterfacesRepositorios;

public interface IRepositorioHistoria : IRepositorio<Historia>
{
    IEnumerable<Historia> FindByEstado(EstadoHistoria estado);
    IEnumerable<Historia> FindByCategoria(int categoriaId);
}
