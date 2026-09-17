using Dominio.Entidades;
using Dominio.InterfacesRepositorios;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos.RepositorioEntityFramework.Repositorios
{
    public class RepositorioUsuarioEF : IRepositorioUsuario
    {
        private readonly HistoriasContext contexto = new HistoriasContext();

        public void Add(Usuario obj)
        {
            contexto.Usuarios.Add(obj);
            contexto.SaveChanges();
        }

        public IEnumerable<Usuario> FindAll()
        {
            return contexto.Usuarios.ToList();
        }

        public Usuario FindByID(int id)
        {
            return contexto.Usuarios.FirstOrDefault(u => u.Id == id);
        }

        public Usuario FindByNombreUsuario(string nombreUsuario)
        {
            return contexto.Usuarios.FirstOrDefault(u => u.NombreUsuario == nombreUsuario);
        }

        public void Remove(int id)
        {
            var usuario = contexto.Usuarios.Find(id);
            if (usuario is not null)
            {
                contexto.Usuarios.Remove(usuario);
                contexto.SaveChanges();
            }
        }

        public void Update(Usuario obj)
        {
            contexto.Usuarios.Update(obj);
            contexto.SaveChanges();
        }
    }
}
