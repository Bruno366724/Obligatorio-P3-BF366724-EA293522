using Dominio.InterfacesRepositorios;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas;

namespace LogicaAplicacion.CasosDeUso.Lecturas;

// Lecturas de un usuario en un mes y año dados.
public class EncontrarHistorialLecturasCU : IEncontrarHistorialLecturas
{
    private readonly IRepositorioLectura repositorio;

    public EncontrarHistorialLecturasCU(IRepositorioLectura repo)
    {
        repositorio = repo;
    }

    public List<LecturaDTO> Ejecutar(int usuarioId, int mes, int anio)
    {
        return repositorio.FindByUsuarioYPeriodo(usuarioId, mes, anio)
            .Select(lectura => LecturaMapper.ToDTO(lectura))
            .ToList();
    }
}
