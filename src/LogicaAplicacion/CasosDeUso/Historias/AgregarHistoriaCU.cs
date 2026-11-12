using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfacesRepositorios;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Auditorias;
using LogicaAplicacion.InterfacesDeCasoDeUso.Historias;

namespace LogicaAplicacion.CasosDeUso.Historias;

public class AgregarHistoriaCU : IAgregarHistoria
{
    private readonly IRepositorioHistoria repositorio;
    private readonly IAgregarAuditoria agregarAuditoria;

    public AgregarHistoriaCU(IRepositorioHistoria repo, IAgregarAuditoria agregarAuditoriaCu)
    {
        repositorio = repo;
        agregarAuditoria = agregarAuditoriaCu;
    }

    public void Ejecutar(HistoriaDTO dto, int administradorId)
    {
        if (dto.Capitulos.Count != 1)
            throw new HistoriaException("La historia debe crearse con exactamente un capítulo inicial.");

        if (dto.Capitulos.Single() is not CapituloIntermedioDTO capituloInicialDto)
            throw new HistoriaException("El capítulo inicial de una historia debe ser de tipo intermedio.");

        // Valida titulo y texto del capitulo inicial (nace sin opciones, no se valida completo).
        if (string.IsNullOrWhiteSpace(capituloInicialDto.Titulo) || string.IsNullOrWhiteSpace(capituloInicialDto.Texto))
            throw new HistoriaException("El título y el texto del capítulo inicial son obligatorios.");

        CapituloIntermedio capituloInicial = (CapituloIntermedio)CapituloMapper.FromDTO(capituloInicialDto);

        Historia historia = new Historia
        {
            Titulo = dto.Titulo,
            Sinopsis = dto.Sinopsis,
            Categorias = dto.Categorias.Select(CategoriaMapper.FromDTO).ToList(),
            Capitulos = new List<Capitulo> { capituloInicial },
            CapituloInicial = capituloInicial
        };

        historia.Validar();
        repositorio.Add(historia);

        // Registra la auditoria del alta de historia.
        agregarAuditoria.Ejecutar(new AuditoriaDTO
        {
            Accion = "Creación de historia",
            AdministradorId = administradorId,
            HistoriaId = historia.Id
        });
    }
}
