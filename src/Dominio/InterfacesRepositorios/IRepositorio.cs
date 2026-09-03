namespace Dominio.InterfacesRepositorios;

public interface IRepositorio<T> where T : class
{
    IEnumerable<T> FindAll();
    T? FindByID(int id);
    bool Add(T obj);
    bool Remove(int id);
    bool Update(T obj);
}
