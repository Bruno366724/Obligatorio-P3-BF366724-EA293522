using Dominio.Entidades;
using Dominio.Enumerados;
using Dominio.InterfacesRepositorios;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos.RepositorioEntityFramework.Repositorios
{
    public class RepositorioHistoriaEF : IRepositorioHistoria
    {
        private readonly HistoriasContext contexto = new HistoriasContext();

        private IQueryable<Historia> ConDetalle()
        {
            return contexto.Historias
                .Include(h => h.Categorias)
                .Include(h => h.Capitulos)
                .Include(h => h.CapituloInicial);
        }

        public void Add(Historia obj)
        {
            contexto.Historias.Add(obj);
            contexto.SaveChanges();
        }

        public IEnumerable<Historia> FindAll()
        {
            return ConDetalle().ToList();
        }

        public Historia FindByID(int id)
        {
            return ConDetalle().FirstOrDefault(h => h.Id == id);
        }

        public IEnumerable<Historia> FindByEstado(EstadoHistoria estado)
        {
            return ConDetalle().Where(h => h.Estado == estado).ToList();
        }

        public IEnumerable<Historia> FindByCategoria(int categoriaId)
        {
            return ConDetalle().Where(h => h.Categorias.Any(c => c.Id == categoriaId)).ToList();
        }

        public void Remove(int id)
        {
            var historia = contexto.Historias.Find(id);
            if (historia is not null)
            {
                contexto.Historias.Remove(historia);
                contexto.SaveChanges();
            }
        }

        public void Update(Historia obj)
        {
            contexto.Historias.Update(obj);
            contexto.SaveChanges();
        }
    }
}
