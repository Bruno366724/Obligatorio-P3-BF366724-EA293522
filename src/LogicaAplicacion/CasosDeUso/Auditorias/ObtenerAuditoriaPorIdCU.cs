using Dominio.InterfacesRepositorios;
using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Auditorias;

namespace LogicaAplicacion.CasosDeUso.Auditorias;

public class ObtenerAuditoriaPorIdCU : IObtenerAuditoriaPorId
{
    private readonly IRepositorioAuditoria repositorio;

    public ObtenerAuditoriaPorIdCU(IRepositorioAuditoria repo)
    {
        repositorio = repo;
    }

    public AuditoriaDTO Ejecutar(int id)
    {
        throw new NotImplementedException();
    }
}
