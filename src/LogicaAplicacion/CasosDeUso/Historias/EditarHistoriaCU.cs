using Dominio.Entidades;
using Dominio.Enumerados;
using Dominio.Excepciones;
using Dominio.InterfacesRepositorios;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Historias;

namespace LogicaAplicacion.CasosDeUso.Historias;

public class EditarHistoriaCU : IEditarHistoria
{
    private readonly IRepositorioHistoria repositorio;

    public EditarHistoriaCU(IRepositorioHistoria repo)
    {
        repositorio = repo;
    }

    public void Ejecutar(HistoriaDTO dto)
    {
        Historia historia = repositorio.FindByID(dto.Id);
        if (historia is null)
            throw new HistoriaException("No se encontró la historia solicitada.");

        historia.Titulo = dto.Titulo;
        historia.Sinopsis = dto.Sinopsis;
        historia.Categorias = dto.Categorias.Select(CategoriaMapper.FromDTO).ToList();

        if (dto.Estado == EstadoHistoria.Publicada)
        {
            historia.ValidarParaPublicar();
        }
        else
        {
            historia.Estado = dto.Estado;
            historia.Validar();
        }

        repositorio.Update(historia);
    }
}
