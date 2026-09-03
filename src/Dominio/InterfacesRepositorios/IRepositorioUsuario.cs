using Dominio.Entidades;

namespace Dominio.InterfacesRepositorios;

public interface IRepositorioUsuario : IRepositorio<Usuario>
{
    Usuario? FindByNombreUsuario(string nombreUsuario);
}
