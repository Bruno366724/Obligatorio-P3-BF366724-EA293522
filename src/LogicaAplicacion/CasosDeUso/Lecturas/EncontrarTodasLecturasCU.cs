using Dominio.InterfacesRepositorios;
using DTOs.DTOs;
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
        throw new NotImplementedException();
    }
}
