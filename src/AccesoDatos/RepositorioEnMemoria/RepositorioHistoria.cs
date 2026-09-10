using Dominio.Entidades;
using Dominio.Enumerados;
using Dominio.InterfacesRepositorios;

namespace AccesoDatos.Repositorios;

public class RepositorioHistoria : IRepositorioHistoria
{
    public IEnumerable<Historia> FindAll()
    {
        throw new NotImplementedException();
    }

    public Historia FindByID(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Historia> FindByEstado(EstadoHistoria estado)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Historia> FindByCategoria(int categoriaId)
    {
        throw new NotImplementedException();
    }

    public void Add(Historia obj)
    {
        throw new NotImplementedException();
    }

    public void Remove(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(Historia obj)
    {
        throw new NotImplementedException();
    }
}
