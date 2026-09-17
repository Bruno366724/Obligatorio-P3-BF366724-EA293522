using Dominio.Entidades;
using Dominio.InterfacesRepositorios;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos.RepositorioEntityFramework.Repositorios
{
    public class RepositorioAuditoriaEF : IRepositorioAuditoria
    {
        private readonly HistoriasContext contexto = new HistoriasContext();

        private IQueryable<Auditoria> ConDetalle()
        {
            return contexto.Auditorias
                .Include(a => a.Administrador)
                .Include(a => a.Historia)
                .Include(a => a.Capitulo);
        }

        public void Add(Auditoria obj)
        {
            contexto.Auditorias.Add(obj);
            contexto.SaveChanges();
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
            var auditoria = contexto.Auditorias.Find(id);
            if (auditoria is not null)
            {
                contexto.Auditorias.Remove(auditoria);
                contexto.SaveChanges();
            }
        }

        public void Update(Auditoria obj)
        {
            contexto.Auditorias.Update(obj);
            contexto.SaveChanges();
        }
    }
}
