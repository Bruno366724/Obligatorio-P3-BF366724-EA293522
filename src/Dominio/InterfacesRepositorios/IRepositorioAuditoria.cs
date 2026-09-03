using Dominio.Entidades;

namespace Dominio.InterfacesRepositorios;

public interface IRepositorioAuditoria : IRepositorio<Auditoria>
{
    IEnumerable<Auditoria> FindByHistoria(int historiaId);
    IEnumerable<Auditoria> FindByAdministrador(int administradorId);
}
