using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfacesRepositorios;
using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Auditorias;
using LogicaAplicacion.InterfacesDeCasoDeUso.Historias;

namespace LogicaAplicacion.CasosDeUso.Historias;

public class EliminarHistoriaCU : IEliminarHistoria
{
    private readonly IRepositorioHistoria repositorioHistoria;
    private readonly IRepositorioLectura repositorioLectura;
    private readonly IAgregarAuditoria agregarAuditoria;

    public EliminarHistoriaCU(IRepositorioHistoria repoHistoria, IRepositorioLectura repoLectura, IAgregarAuditoria agregarAuditoriaCu)
    {
        repositorioHistoria = repoHistoria;
        repositorioLectura = repoLectura;
        agregarAuditoria = agregarAuditoriaCu;
    }

    public void Ejecutar(int historiaId, int administradorId)
    {
        Historia historia = repositorioHistoria.FindByID(historiaId);
        if (historia is null)
            throw new HistoriaNoEncontradaException("No se encontró la historia solicitada.");

        // Regla de RF03: no se da de baja una historia con lecturas asociadas.
        if (repositorioLectura.ExistenDeHistoria(historiaId))
            throw new HistoriaException($"No se puede eliminar \"{historia.Titulo}\" porque ya tiene lecturas asociadas.");

        string titulo = historia.Titulo;
        repositorioHistoria.Remove(historiaId);

        // Auditoria de la baja, con el titulo porque la historia ya no existe.
        agregarAuditoria.Ejecutar(new AuditoriaDTO
        {
            Accion = $"Baja de historia: {titulo}",
            AdministradorId = administradorId,
            HistoriaId = null
        });
    }
}
