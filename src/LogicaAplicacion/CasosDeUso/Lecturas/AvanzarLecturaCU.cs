using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfacesRepositorios;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas;

namespace LogicaAplicacion.CasosDeUso.Lecturas;

public class AvanzarLecturaCU : IAvanzarLectura
{
    private readonly IRepositorioLectura repositorio;

    public AvanzarLecturaCU(IRepositorioLectura repo)
    {
        repositorio = repo;
    }

    public LecturaDTO Ejecutar(int lecturaId, int opcionId, int usuarioId)
    {
        Lectura lectura = repositorio.FindByID(lecturaId);
        if (lectura is null)
            throw new LecturaException("No se encontró la lectura solicitada.");

        // La lectura tiene que ser del usuario que pide avanzarla.
        if (lectura.UsuarioId != usuarioId)
            throw new LecturaException("Esta lectura no te pertenece.");

        // Una lectura terminada no se puede seguir avanzando.
        if (lectura.FechaFin.HasValue)
            throw new LecturaException("Esta lectura ya terminó. Iniciá una nueva para probar otro camino.");

        if (lectura.CapituloActual is not CapituloIntermedio capituloActual)
            throw new LecturaException("No hay opciones para avanzar desde el capítulo actual.");

        Opcion opcionElegida = capituloActual.Opciones.FirstOrDefault(opcion => opcion.Id == opcionId);
        if (opcionElegida is null)
            throw new LecturaException("La opción elegida no pertenece al capítulo actual.");

        Capitulo capituloDestino = opcionElegida.CapituloDestino;
        lectura.CapituloActualId = capituloDestino.Id;
        lectura.CapituloActual = capituloDestino;

        // Si el destino es un final, la lectura se cierra y se cuenta el alcance.
        if (capituloDestino is CapituloFinal final)
        {
            final.RegistrarAlcance();
            lectura.FechaFin = DateTime.Now;
            lectura.FinalAlcanzadoId = final.Id;
            lectura.FinalAlcanzado = final;
        }

        lectura.Validar();
        repositorio.Update(lectura);

        return LecturaMapper.ToDTO(lectura);
    }
}
