using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfacesRepositorios;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas;

namespace LogicaAplicacion.CasosDeUso.Lecturas;

public class ObtenerLecturaPorIdCU : IObtenerLecturaPorId
{
    private readonly IRepositorioLectura repositorio;

    public ObtenerLecturaPorIdCU(IRepositorioLectura repo)
    {
        repositorio = repo;
    }

    public LecturaDTO Ejecutar(int id)
    {
        Lectura lectura = repositorio.FindByID(id);
        if (lectura is null)
            throw new LecturaException("No se encontró la lectura solicitada.");

        return LecturaMapper.ToDTO(lectura);
    }
}
