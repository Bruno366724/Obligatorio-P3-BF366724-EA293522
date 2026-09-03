using Dominio.Entidades;

namespace Dominio.InterfacesRepositorios;

public interface IRepositorioLectura : IRepositorio<Lectura>
{
    IEnumerable<Lectura> FindByUsuarioYPeriodo(int usuarioId, int mes, int anio);
}
