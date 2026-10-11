using Dominio.InterfacesRepositorios;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas;

namespace LogicaAplicacion.CasosDeUso.Lecturas;

public class EncontrarTodasLecturasCU : IEncontrarTodasLecturas
{
    private readonly IRepositorioLectura repositorio;

    public EncontrarTodasLecturasCU(IRepositorioLectura repo)
    {
        repositorio = repo;
    }

    public List<LecturaDTO> Ejecutar()
    {
        return repositorio.FindAll()
            .Select(lectura => LecturaMapper.ToDTO(lectura))
            .ToList();
    }
}
