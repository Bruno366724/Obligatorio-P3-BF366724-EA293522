using Dominio.Entidades;

namespace Dominio.Interfaces.Repositorios;

public interface IAuditoriaRepository : IRepositorio<Auditoria>
{
    IEnumerable<Auditoria> ObtenerPorHistoria(int historiaId);
    IEnumerable<Auditoria> ObtenerPorAdministrador(int administradorId);
}
