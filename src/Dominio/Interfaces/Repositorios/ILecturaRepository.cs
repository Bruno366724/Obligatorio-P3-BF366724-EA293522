using Dominio.Entidades;

namespace Dominio.Interfaces.Repositorios;

public interface ILecturaRepository : IRepositorio<Lectura>
{
    IEnumerable<Lectura> ObtenerPorUsuarioYPeriodo(int usuarioId, int mes, int anio);
}
