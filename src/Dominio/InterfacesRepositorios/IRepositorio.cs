namespace Dominio.InterfacesRepositorios;

public interface IRepositorio<T> where T : class
{
    IEnumerable<T> FindAll();
    T FindByID(int id);
    void Add(T obj);
    void Remove(int id);
    void Update(T obj);
}
