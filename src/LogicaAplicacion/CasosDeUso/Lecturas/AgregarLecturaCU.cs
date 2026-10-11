using Dominio.Entidades;
using Dominio.InterfacesRepositorios;
using DTOs.DTOs;
using DTOs.Mappers;
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
        Lectura lectura = LecturaMapper.FromDTO(dto);
        lectura.Validar();
        repositorio.Add(lectura);
    }
}
