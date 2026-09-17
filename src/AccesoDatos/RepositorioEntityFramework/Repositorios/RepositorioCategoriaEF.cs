using Dominio.Entidades;
using Dominio.InterfacesRepositorios;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos.RepositorioEntityFramework.Repositorios
{
    public class RepositorioCategoriaEF : IRepositorioCategoria
    {
        private readonly HistoriasContext contexto = new HistoriasContext();

        public void Add(Categoria obj)
        {
            contexto.Categorias.Add(obj);
            contexto.SaveChanges();
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
            Categoria? categoria = contexto.Categorias.Find(id);
            if (categoria is not null)
            {
                contexto.Categorias.Remove(categoria);
                contexto.SaveChanges();
            }
        }

        public void Update(Categoria obj)
        {
            contexto.Categorias.Update(obj);
            contexto.SaveChanges();
        }
    }
}
