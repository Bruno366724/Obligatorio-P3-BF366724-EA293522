using Dominio.InterfacesRepositorios;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Auditorias;

namespace LogicaAplicacion.CasosDeUso.Auditorias;

public class EncontrarTodasAuditoriasCU : IEncontrarTodasAuditorias
{
    private readonly IRepositorioAuditoria repositorio;

    public EncontrarTodasAuditoriasCU(IRepositorioAuditoria repo)
    {
        repositorio = repo;
    }

    public List<AuditoriaDTO> Ejecutar()
    {
        return repositorio.FindAll()
            .Select(auditoria => AuditoriaMapper.ToDTO(auditoria))
            .ToList();
    }
}
