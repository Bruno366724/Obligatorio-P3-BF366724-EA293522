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
            throw new HistoriaNoEncontradaException("No se encontró la historia solicitada.");

        EstadoHistoria estadoAnterior = historia.Estado;

        historia.Titulo = dto.Titulo;
        historia.Sinopsis = dto.Sinopsis;
        historia.Categorias = dto.Categorias.Select(CategoriaMapper.FromDTO).ToList();

        Capitulo? capituloInicialEditado = EditarCapituloInicial(historia, dto);

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

        // Registra la auditoria de la edicion del capitulo inicial, si cambio.
        if (capituloInicialEditado is not null)
        {
            agregarAuditoria.Ejecutar(new AuditoriaDTO
            {
                Accion = "Edición de capítulo inicial",
                AdministradorId = administradorId,
                HistoriaId = historia.Id,
                CapituloId = capituloInicialEditado.Id
            });
        }
    }

    // Modifica titulo y texto del capitulo inicial; solo si la historia no tiene otros capitulos.
    private Capitulo? EditarCapituloInicial(Historia historia, HistoriaDTO dto)
    {
        CapituloDTO? capituloDto = dto.Capitulos.FirstOrDefault();
        if (capituloDto is null)
            return null;

        if (historia.Capitulos.Count > 1)
            throw new HistoriaException("No se puede modificar el capítulo inicial porque la historia ya tiene otros capítulos.");

        if (string.IsNullOrWhiteSpace(capituloDto.Titulo) || string.IsNullOrWhiteSpace(capituloDto.Texto))
            throw new HistoriaException("El título y el texto del capítulo inicial son obligatorios.");

        Capitulo capituloInicial = historia.Capitulos.Single(capitulo => capitulo.Id == historia.CapituloInicialId);
        if (capituloInicial.Titulo == capituloDto.Titulo && capituloInicial.Texto == capituloDto.Texto)
            return null;

        capituloInicial.Titulo = capituloDto.Titulo;
        capituloInicial.Texto = capituloDto.Texto;
        return capituloInicial;
    }
}
