using Dominio.InterfacesRepositorios;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Auditorias;

namespace LogicaAplicacion.CasosDeUso.Auditorias;

public class EncontrarAuditoriasPorHistoriaCU : IEncontrarAuditoriasPorHistoria
{
    private readonly IRepositorioAuditoria repositorio;

    public EncontrarAuditoriasPorHistoriaCU(IRepositorioAuditoria repo)
    {
        repositorio = repo;
    }

    public List<AuditoriaDTO> Ejecutar(int historiaId)
    {
        return repositorio.FindByHistoria(historiaId)
            .Select(auditoria => AuditoriaMapper.ToDTO(auditoria))
            .ToList();
    }
}
