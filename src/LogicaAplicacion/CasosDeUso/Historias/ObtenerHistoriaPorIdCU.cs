using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfacesRepositorios;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Historias;

namespace LogicaAplicacion.CasosDeUso.Historias;

public class ObtenerHistoriaPorIdCU : IObtenerHistoriaPorId
{
    private readonly IRepositorioHistoria repositorio;

    public ObtenerHistoriaPorIdCU(IRepositorioHistoria repo)
    {
        repositorio = repo;
    }

    public HistoriaDTO Ejecutar(int id)
    {
        Historia historia = repositorio.FindByID(id);
        if (historia is null)
            throw new HistoriaException("No se encontró la historia solicitada.");

        return HistoriaMapper.ToDTO(historia);
    }
}
