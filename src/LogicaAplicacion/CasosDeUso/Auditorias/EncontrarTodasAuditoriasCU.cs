using Dominio.InterfacesRepositorios;
using DTOs.DTOs;
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
        throw new NotImplementedException();
    }
}
