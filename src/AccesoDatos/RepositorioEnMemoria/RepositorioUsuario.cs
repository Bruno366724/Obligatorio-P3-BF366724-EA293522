using Dominio.Entidades;
using Dominio.InterfacesRepositorios;

namespace AccesoDatos.Repositorios;

public class RepositorioUsuario : IRepositorioUsuario
{

    private static List<Usuario> usuarios = new List<Usuario>();

    public IEnumerable<Usuario> FindAll()
    {
        return new List<Usuario>(usuarios);
    }

    public Usuario FindByID(int id)
    {
        throw new NotImplementedException();
    }

    public Usuario FindByNombreUsuario(string nombreUsuario)
    {
        throw new NotImplementedException();
    }

    public void Add(Usuario obj)
    {
        throw new NotImplementedException();
    }

    public void Remove(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(Usuario obj)
    {
        throw new NotImplementedException();
    }
}
