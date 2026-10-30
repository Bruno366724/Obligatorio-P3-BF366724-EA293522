using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfacesRepositorios;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos.RepositorioEntityFramework.Repositorios
{
    public class RepositorioCategoriaEF : IRepositorioCategoria
    {
        private readonly HistoriasContext contexto;

        public RepositorioCategoriaEF(HistoriasContext context)
        {
            contexto = context;
        }

        public void Add(Categoria obj)
        {
            try
            {
                contexto.Categorias.Add(obj);
                contexto.SaveChanges();
            }
            catch (Exception)
            {
                throw new CategoriaException("Error al guardar en la base de datos.");
            }
        }

        public IEnumerable<Categoria> FindAll()
        {
            return contexto.Categorias.ToList();
        }

        public Categoria FindByID(int id)
        {
            return contexto.Categorias.FirstOrDefault(c => c.Id == id);
        }

        public void Remove(int id)
        {
            try
            {
                Categoria? categoria = contexto.Categorias.Find(id);
                if (categoria is not null)
                {
                    contexto.Categorias.Remove(categoria);
                    contexto.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw new CategoriaException("Error al eliminar en la base de datos.");
            }
        }

        public void Update(Categoria obj)
        {
            try
            {
                contexto.Categorias.Update(obj);
                contexto.SaveChanges();
            }
            catch (Exception)
            {
                throw new CategoriaException("Error al guardar en la base de datos.");
            }
        }
    }
}
