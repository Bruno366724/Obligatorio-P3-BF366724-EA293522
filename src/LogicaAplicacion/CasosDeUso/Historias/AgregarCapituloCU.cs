using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfacesRepositorios;
using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Auditorias;
using LogicaAplicacion.InterfacesDeCasoDeUso.Historias;

namespace LogicaAplicacion.CasosDeUso.Historias;

public class AgregarCapituloCU : IAgregarCapitulo
{
    private readonly IRepositorioHistoria repositorio;
    private readonly IAgregarAuditoria agregarAuditoria;

    public AgregarCapituloCU(IRepositorioHistoria repo, IAgregarAuditoria agregarAuditoriaCu)
    {
        repositorio = repo;
        agregarAuditoria = agregarAuditoriaCu;
    }

    public void Ejecutar(int historiaId, int capituloAntecedenteId, bool esFinal, string titulo, string texto, string textoOpcion, int administradorId)
    {
        Historia historia = repositorio.FindByID(historiaId);
        if (historia is null)
            throw new HistoriaException("No se encontró la historia solicitada.");

        // El antecedente tiene que ser un capitulo de esta misma historia, de tipo intermedio.
        Capitulo antecedente = historia.Capitulos.FirstOrDefault(capitulo => capitulo.Id == capituloAntecedenteId);
        if (antecedente is null)
            throw new HistoriaException("El capítulo antecedente no pertenece a esta historia.");

        if (antecedente is not CapituloIntermedio capituloIntermedioAntecedente)
            throw new HistoriaException("Solo se pueden agregar capítulos a continuación de un capítulo intermedio.");

        // Capitulo nuevo (sin validar completo todavia si es intermedio: nace sin opciones).
        Capitulo nuevoCapitulo = esFinal
            ? new CapituloFinal { Titulo = titulo, Texto = texto, HistoriaId = historia.Id }
            : new CapituloIntermedio { Titulo = titulo, Texto = texto, HistoriaId = historia.Id };

        if (esFinal)
            nuevoCapitulo.Validar();
        else if (string.IsNullOrWhiteSpace(titulo) || string.IsNullOrWhiteSpace(texto))
            throw new HistoriaException("El título y el texto del capítulo son obligatorios.");

        historia.Capitulos.Add(nuevoCapitulo);

        // Opcion nueva en el antecedente, apuntando al capitulo nuevo.
        Opcion nuevaOpcion = new Opcion { Texto = textoOpcion, CapituloDestino = nuevoCapitulo };
        nuevaOpcion.Validar();
        capituloIntermedioAntecedente.Opciones.Add(nuevaOpcion);

        repositorio.Update(historia);

        // Registra la auditoria del alta de capitulo.
        agregarAuditoria.Ejecutar(new AuditoriaDTO
        {
            Accion = "Alta de capítulo",
            AdministradorId = administradorId,
            HistoriaId = historia.Id,
            CapituloId = nuevoCapitulo.Id
        });
    }
}
