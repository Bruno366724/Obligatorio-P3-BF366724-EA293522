using Dominio.Entidades;
using Dominio.Enumerados;
using Dominio.Excepciones;
using Dominio.InterfacesRepositorios;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas;

namespace LogicaAplicacion.CasosDeUso.Lecturas;

public class IniciarLecturaCU : IIniciarLectura
{
    private readonly IRepositorioLectura repositorioLectura;
    private readonly IRepositorioHistoria repositorioHistoria;

    // Lee Historia (estado y capitulo inicial) y escribe Lectura.
    public IniciarLecturaCU(IRepositorioLectura repoLectura, IRepositorioHistoria repoHistoria)
    {
        repositorioLectura = repoLectura;
        repositorioHistoria = repoHistoria;
    }

    public LecturaDTO Ejecutar(int usuarioId, int historiaId)
    {
        Historia historia = repositorioHistoria.FindByID(historiaId);
        if (historia is null)
            throw new LecturaException("No se encontró la historia solicitada.");

        if (historia.Estado != EstadoHistoria.Publicada)
            throw new LecturaException("Solo se pueden leer historias publicadas.");

        if (historia.CapituloInicialId is null)
            throw new LecturaException("La historia no tiene un capítulo inicial definido.");

        Lectura lectura = new Lectura
        {
            UsuarioId = usuarioId,
            HistoriaId = historiaId,
            CapituloActualId = historia.CapituloInicialId.Value,
            FechaInicio = DateTime.Now
        };

        lectura.Validar();
        repositorioLectura.Add(lectura);

        return LecturaMapper.ToDTO(lectura);
    }
}
