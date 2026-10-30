using Dominio.Entidades;
using Dominio.InterfacesRepositorios;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos.RepositorioEntityFramework.Repositorios
{
    public class RepositorioLecturaEF : IRepositorioLectura
    {
        private readonly HistoriasContext contexto;

        public RepositorioLecturaEF(HistoriasContext context)
        {
            contexto = context;
        }

        private IQueryable<Lectura> ConDetalle()
        {
            // Includes por string: Opciones/CapituloDestino solo existen en CapituloIntermedio (TPH).
            return contexto.Lecturas
                .Include(l => l.Usuario)
                .Include(l => l.Historia)
                .Include(l => l.CapituloActual)
                .Include("CapituloActual.Opciones")
                .Include("CapituloActual.Opciones.CapituloDestino")
                .Include(l => l.FinalAlcanzado);
        }

        public void Add(Lectura obj)
        {
            contexto.Lecturas.Add(obj);
            contexto.SaveChanges();
        }

        public IEnumerable<Lectura> FindAll()
        {
            return ConDetalle().ToList();
        }

        public Lectura FindByID(int id)
        {
            return ConDetalle().FirstOrDefault(l => l.Id == id);
        }

        public IEnumerable<Lectura> FindByUsuarioYPeriodo(int usuarioId, int mes, int anio)
        {
            return ConDetalle()
                .Where(l => l.UsuarioId == usuarioId
                    && l.FechaInicio.Month == mes
                    && l.FechaInicio.Year == anio)
                .ToList();
        }

        // Lecturas que llegaron a un final.
        public IEnumerable<Lectura> FindCompletadas()
        {
            return contexto.Lecturas
                .Include(l => l.Historia)
                .Include(l => l.FinalAlcanzado)
                .Where(l => l.FechaFin != null)
                .ToList();
        }

        public void Remove(int id)
        {
            Lectura? lectura = contexto.Lecturas.Find(id);
            if (lectura is not null)
            {
                contexto.Lecturas.Remove(lectura);
                contexto.SaveChanges();
            }
        }

        public void Update(Lectura obj)
        {
            contexto.Lecturas.Update(obj);
            contexto.SaveChanges();
        }
    }
}
