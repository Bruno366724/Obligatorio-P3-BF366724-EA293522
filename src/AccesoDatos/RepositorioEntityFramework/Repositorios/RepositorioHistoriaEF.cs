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
            // Include por string: Opciones solo existe en CapituloIntermedio (TPH).
            return contexto.Historias
                .Include(h => h.Categorias)
                .Include(h => h.Capitulos)
                .Include("Capitulos.Opciones")
                .Include(h => h.CapituloInicial);
        }

        private List<Categoria> ResolverCategorias(List<Categoria> categorias)
        {
            List<int> ids = categorias.Select(categoria => categoria.Id).ToList();
            return contexto.Categorias.Where(categoria => ids.Contains(categoria.Id)).ToList();
        }

        public void Add(Historia obj)
        {
            obj.Categorias = ResolverCategorias(obj.Categorias);

            // Guardado en dos pasos por el ciclo de FKs Historia<->CapituloInicial.
            CapituloIntermedio? capituloInicial = obj.CapituloInicial;
            obj.CapituloInicial = null;

            contexto.Historias.Add(obj);
            contexto.SaveChanges();

            obj.CapituloInicial = capituloInicial;
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
            Historia? historia = contexto.Historias.Find(id);
            if (historia is not null)
            {
                contexto.Historias.Remove(historia);
                contexto.SaveChanges();
            }
        }

        public void Update(Historia obj)
        {
            obj.Categorias = ResolverCategorias(obj.Categorias);
            contexto.Historias.Update(obj);
            contexto.SaveChanges();
        }
    }
}
