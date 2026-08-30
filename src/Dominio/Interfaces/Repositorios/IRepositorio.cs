namespace Dominio.Interfaces.Repositorios;

public interface IRepositorio<T> where T : class
{
    T Alta(T entidad);
    void Modificacion(T entidad);
    void Baja(int id);
    T? ObtenerPorId(int id);
    IEnumerable<T> ObtenerTodos();
}
