using Dominio.Entidades;
using Dominio.Enumerados;
using Dominio.InterfacesRepositorios;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Historias;

namespace LogicaAplicacion.CasosDeUso.Historias;

// Historias en estado Publicada, filtrables por categoria.
public class EncontrarHistoriasPublicadasCU : IEncontrarHistoriasPublicadas
{
    private readonly IRepositorioHistoria repositorio;

    public EncontrarHistoriasPublicadasCU(IRepositorioHistoria repo)
    {
        repositorio = repo;
    }

    public List<HistoriaDTO> Ejecutar(int? categoriaId)
    {
        IEnumerable<Historia> publicadas = repositorio.FindByEstado(EstadoHistoria.Publicada);

        if (categoriaId.HasValue)
            publicadas = publicadas.Where(historia => historia.Categorias.Any(categoria => categoria.Id == categoriaId.Value));

        return publicadas
            .Select(historia => HistoriaMapper.ToDTO(historia))
            .ToList();
    }
}
