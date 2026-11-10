using Dominio.Entidades;
using Dominio.Enumerados;
using Dominio.Excepciones;
using Dominio.InterfacesRepositorios;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace AccesoDatos.RepositorioEntityFramework.Repositorios
{
    public class RepositorioHistoriaEF : IRepositorioHistoria
    {
        private readonly HistoriasContext contexto;

        public RepositorioHistoriaEF(HistoriasContext context)
        {
            contexto = context;
        }

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
            try
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
            catch (Exception)
            {
                throw new HistoriaException("Error al guardar en la base de datos.");
            }
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
            try
            {
                Historia? historia = ConDetalle().FirstOrDefault(h => h.Id == id);
                if (historia is not null)
                {
                    using IDbContextTransaction transaccion = contexto.Database.BeginTransaction();

                    // Desvincula las auditorias de la historia y de sus capitulos.
                    contexto.Auditorias
                        .Where(a => a.HistoriaId == id)
                        .ExecuteUpdate(setters => setters
                            .SetProperty(a => a.HistoriaId, (int?)null)
                            .SetProperty(a => a.CapituloId, (int?)null));

                    // Borrado en dos pasos por el ciclo de FKs Historia<->CapituloInicial.
                    historia.CapituloInicialId = null;
                    historia.CapituloInicial = null;
                    contexto.SaveChanges();

                    foreach (CapituloIntermedio intermedio in historia.Capitulos.OfType<CapituloIntermedio>())
                        contexto.Opciones.RemoveRange(intermedio.Opciones);

                    contexto.Capitulos.RemoveRange(historia.Capitulos);
                    contexto.Historias.Remove(historia);
                    contexto.SaveChanges();

                    transaccion.Commit();
                }
            }
            catch (Exception)
            {
                throw new HistoriaException("Error al eliminar en la base de datos.");
            }
        }

        public void Update(Historia obj)
        {
            try
            {
                obj.Categorias = ResolverCategorias(obj.Categorias);
                contexto.Historias.Update(obj);
                contexto.SaveChanges();
            }
            catch (Exception)
            {
                throw new HistoriaException("Error al guardar en la base de datos.");
            }
        }
    }
}
