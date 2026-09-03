using Dominio.Entidades;
using Dominio.InterfacesRepositorios;

namespace AccesoDatos.Repositorios;

public class RepositorioAuditoria : IRepositorioAuditoria
{
    public IEnumerable<Auditoria> FindAll()
    {
        throw new NotImplementedException();
    }

    public Auditoria FindByID(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Auditoria> FindByHistoria(int historiaId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Auditoria> FindByAdministrador(int administradorId)
    {
        throw new NotImplementedException();
    }

    public void Add(Auditoria obj)
    {
        throw new NotImplementedException();
    }

    public void Remove(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(Auditoria obj)
    {
        throw new NotImplementedException();
    }
}
