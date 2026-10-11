using Dominio.Entidades;
using Dominio.Enumerados;
using Dominio.Excepciones;
using Dominio.InterfacesRepositorios;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Auditorias;
using LogicaAplicacion.InterfacesDeCasoDeUso.Historias;

namespace LogicaAplicacion.CasosDeUso.Historias;

public class EditarHistoriaCU : IEditarHistoria
{
    private readonly IRepositorioHistoria repositorio;
    private readonly IAgregarAuditoria agregarAuditoria;

    public EditarHistoriaCU(IRepositorioHistoria repo, IAgregarAuditoria agregarAuditoriaCu)
    {
        repositorio = repo;
        agregarAuditoria = agregarAuditoriaCu;
    }

    public void Ejecutar(HistoriaDTO dto, int administradorId)
    {
        Historia historia = repositorio.FindByID(dto.Id);
        if (historia is null)
            throw new HistoriaException("No se encontró la historia solicitada.");

        EstadoHistoria estadoAnterior = historia.Estado;

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

        // Es "publicacion" solo si el estado pasa a Publicada en esta llamada.
        bool fuePublicacion = estadoAnterior != EstadoHistoria.Publicada && historia.Estado == EstadoHistoria.Publicada;

        agregarAuditoria.Ejecutar(new AuditoriaDTO
        {
            Accion = fuePublicacion ? "Publicación de historia" : "Edición de historia",
            AdministradorId = administradorId,
            HistoriaId = historia.Id
        });
    }
}
