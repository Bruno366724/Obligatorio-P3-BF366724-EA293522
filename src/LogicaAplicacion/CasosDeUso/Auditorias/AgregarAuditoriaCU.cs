using Dominio.InterfacesRepositorios;
using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Auditorias;

namespace LogicaAplicacion.CasosDeUso.Auditorias;

public class AgregarAuditoriaCU : IAgregarAuditoria
{
    private readonly IRepositorioAuditoria repositorio;

    public AgregarAuditoriaCU(IRepositorioAuditoria repo)
    {
        repositorio = repo;
    }

    public void Ejecutar(AuditoriaDTO dto)
    {
        throw new NotImplementedException();
    }
}
