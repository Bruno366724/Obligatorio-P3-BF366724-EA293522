using Dominio.InterfacesRepositorios;
using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Historias;

namespace LogicaAplicacion.CasosDeUso.Historias;

public class AgregarHistoriaCU : IAgregarHistoria
{
    private readonly IRepositorioHistoria repositorio;

    public AgregarHistoriaCU(IRepositorioHistoria repo)
    {
        repositorio = repo;
    }

    public void Ejecutar(HistoriaDTO dto)
    {
        throw new NotImplementedException();
    }
}
