using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfacesRepositorios;
using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Auditorias;
using LogicaAplicacion.InterfacesDeCasoDeUso.Historias;

namespace LogicaAplicacion.CasosDeUso.Historias;

public class AgregarOpcionCU : IAgregarOpcion
{
    private readonly IRepositorioHistoria repositorio;
    private readonly IAgregarAuditoria agregarAuditoria;

    public AgregarOpcionCU(IRepositorioHistoria repo, IAgregarAuditoria agregarAuditoriaCu)
    {
        repositorio = repo;
        agregarAuditoria = agregarAuditoriaCu;
    }

    public void Ejecutar(AgregarOpcionDTO dto, int administradorId)
    {
        Historia historia = repositorio.FindByID(dto.HistoriaId);
        if (historia is null)
            throw new HistoriaNoEncontradaException("No se encontró la historia solicitada.");

        // Origen y destino tienen que ser capitulos de esta misma historia.
        Capitulo? origen = historia.Capitulos.FirstOrDefault(capitulo => capitulo.Id == dto.CapituloOrigenId);
        Capitulo? destino = historia.Capitulos.FirstOrDefault(capitulo => capitulo.Id == dto.CapituloDestinoId);
        if (origen is null || destino is null)
            throw new HistoriaException("Los dos capítulos tienen que pertenecer a esta historia.");

        if (origen is not CapituloIntermedio intermedioOrigen)
            throw new HistoriaException("Solo un capítulo intermedio puede tener opciones.");

        if (origen.Id == destino.Id)
            throw new HistoriaException("Un capítulo no puede tener una opción hacia sí mismo.");

        if (intermedioOrigen.Opciones.Any(opcion => opcion.CapituloDestinoId == destino.Id))
            throw new HistoriaException($"\"{origen.Titulo}\" ya tiene una opción que lleva a \"{destino.Titulo}\".");

        // Si desde el destino ya se llega al origen, la opcion armaria un ciclo sin final.
        if (SeLlegaDesde(destino, origen.Id, new HashSet<int>()))
            throw new HistoriaException($"Esa opción armaría un ciclo: desde \"{destino.Titulo}\" ya se puede llegar a \"{origen.Titulo}\".");

        Opcion nuevaOpcion = new Opcion { Texto = dto.TextoOpcion, CapituloDestinoId = destino.Id, CapituloDestino = destino };
        nuevaOpcion.Validar();
        intermedioOrigen.Opciones.Add(nuevaOpcion);

        repositorio.Update(historia);

        // Registra la auditoria sobre el capitulo origen.
        agregarAuditoria.Ejecutar(new AuditoriaDTO
        {
            Accion = "Alta de opción",
            AdministradorId = administradorId,
            HistoriaId = historia.Id,
            CapituloId = origen.Id
        });
    }

    // Recorre las opciones desde un capitulo para ver si se llega al capitulo buscado.
    private bool SeLlegaDesde(Capitulo actual, int buscadoId, HashSet<int> visitados)
    {
        if (actual.Id == buscadoId)
            return true;

        if (!visitados.Add(actual.Id) || actual is not CapituloIntermedio intermedio)
            return false;

        return intermedio.Opciones
            .Where(opcion => opcion.CapituloDestino is not null)
            .Any(opcion => SeLlegaDesde(opcion.CapituloDestino!, buscadoId, visitados));
    }
}
