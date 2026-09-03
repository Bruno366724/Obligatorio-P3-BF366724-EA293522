using Dominio.InterfacesRepositorios;
using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas;

namespace LogicaAplicacion.CasosDeUso.Lecturas;

public class AgregarLecturaCU : IAgregarLectura
{
    private readonly IRepositorioLectura repositorio;

    public AgregarLecturaCU(IRepositorioLectura repo)
    {
        repositorio = repo;
    }

    public void Ejecutar(LecturaDTO dto)
    {
        throw new NotImplementedException();
    }
}
