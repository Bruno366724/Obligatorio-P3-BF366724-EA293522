using Dominio.InterfacesRepositorios;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Auditorias;

namespace LogicaAplicacion.CasosDeUso.Auditorias;

public class EncontrarAuditoriasPorAdministradorCU : IEncontrarAuditoriasPorAdministrador
{
    private readonly IRepositorioAuditoria repositorio;

    public EncontrarAuditoriasPorAdministradorCU(IRepositorioAuditoria repo)
    {
        repositorio = repo;
    }

    public List<AuditoriaDTO> Ejecutar(int administradorId)
    {
        return repositorio.FindByAdministrador(administradorId)
            .Select(auditoria => AuditoriaMapper.ToDTO(auditoria))
            .ToList();
    }
}
