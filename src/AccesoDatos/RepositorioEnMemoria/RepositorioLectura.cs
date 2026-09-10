using Dominio.Entidades;
using Dominio.InterfacesRepositorios;

namespace AccesoDatos.Repositorios;

public class RepositorioLectura : IRepositorioLectura
{
    public IEnumerable<Lectura> FindAll()
    {
        throw new NotImplementedException();
    }

    public Lectura FindByID(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Lectura> FindByUsuarioYPeriodo(int usuarioId, int mes, int anio)
    {
        throw new NotImplementedException();
    }

    public void Add(Lectura obj)
    {
        throw new NotImplementedException();
    }

    public void Remove(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(Lectura obj)
    {
        throw new NotImplementedException();
    }
}
