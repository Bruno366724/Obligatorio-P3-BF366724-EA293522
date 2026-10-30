using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfacesRepositorios;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos.RepositorioEntityFramework.Repositorios
{
    public class RepositorioAuditoriaEF : IRepositorioAuditoria
    {
        private readonly HistoriasContext contexto;

        public RepositorioAuditoriaEF(HistoriasContext context)
        {
            contexto = context;
        }

        private IQueryable<Auditoria> ConDetalle()
        {
            return contexto.Auditorias
                .Include(a => a.Administrador)
                .Include(a => a.Historia)
                .Include(a => a.Capitulo);
        }

        public void Add(Auditoria obj)
        {
            try
            {
                contexto.Auditorias.Add(obj);
                contexto.SaveChanges();
            }
            catch (Exception)
            {
                throw new AuditoriaException("Error al guardar en la base de datos.");
            }
        }

        public IEnumerable<Auditoria> FindAll()
        {
            return ConDetalle().ToList();
        }

        public Auditoria FindByID(int id)
        {
            return ConDetalle().FirstOrDefault(a => a.Id == id);
        }

        public IEnumerable<Auditoria> FindByHistoria(int historiaId)
        {
            return ConDetalle().Where(a => a.HistoriaId == historiaId).ToList();
        }

        public IEnumerable<Auditoria> FindByAdministrador(int administradorId)
        {
            return ConDetalle().Where(a => a.AdministradorId == administradorId).ToList();
        }

        public void Remove(int id)
        {
            try
            {
                Auditoria? auditoria = contexto.Auditorias.Find(id);
                if (auditoria is not null)
                {
                    contexto.Auditorias.Remove(auditoria);
                    contexto.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw new AuditoriaException("Error al eliminar en la base de datos.");
            }
        }

        public void Update(Auditoria obj)
        {
            try
            {
                contexto.Auditorias.Update(obj);
                contexto.SaveChanges();
            }
            catch (Exception)
            {
                throw new AuditoriaException("Error al guardar en la base de datos.");
            }
        }
    }
}
