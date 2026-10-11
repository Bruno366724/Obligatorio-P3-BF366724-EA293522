using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfacesRepositorios;
using DTOs.DTOs;
using DTOs.Mappers;
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
        Auditoria auditoria = repositorio.FindByID(id);
        if (auditoria is null)
            throw new AuditoriaException("No se encontró la auditoría solicitada.");

        return AuditoriaMapper.ToDTO(auditoria);
    }
}
