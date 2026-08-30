using Dominio.Entidades;

namespace Dominio.Interfaces.Repositorios;

public interface IUsuarioRepository : IRepositorio<Usuario>
{
    Usuario? ObtenerPorNombreUsuario(string nombreUsuario);
}
