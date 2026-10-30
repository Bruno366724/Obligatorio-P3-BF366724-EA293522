using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfacesRepositorios;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos.RepositorioEntityFramework.Repositorios
{
    public class RepositorioUsuarioEF : IRepositorioUsuario
    {
        private readonly HistoriasContext contexto;

        public RepositorioUsuarioEF(HistoriasContext context)
        {
            contexto = context;
        }

        public void Add(Usuario obj)
        {
            try
            {
                contexto.Usuarios.Add(obj);
                contexto.SaveChanges();
            }
            catch (Exception)
            {
                throw new UsuarioException("Error al guardar en la base de datos.");
            }
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
            try
            {
                Usuario? usuario = contexto.Usuarios.Find(id);
                if (usuario is not null)
                {
                    contexto.Usuarios.Remove(usuario);
                    contexto.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw new UsuarioException("Error al eliminar en la base de datos.");
            }
        }

        public void Update(Usuario obj)
        {
            try
            {
                contexto.Usuarios.Update(obj);
                contexto.SaveChanges();
            }
            catch (Exception)
            {
                throw new UsuarioException("Error al guardar en la base de datos.");
            }
        }
    }
}
