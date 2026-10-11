using Dominio.Entidades;
using Dominio.InterfacesRepositorios;
using DTOs.DTOs;
using DTOs.Mappers;
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
        Auditoria auditoria = AuditoriaMapper.FromDTO(dto);

        // La fecha la pone el sistema, no el DTO que llega.
        auditoria.Fecha = DateTime.Now;

        auditoria.Validar();
        repositorio.Add(auditoria);
    }
}
