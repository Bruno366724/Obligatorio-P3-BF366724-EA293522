using System.Net;
using System.Security.Claims;
using Dominio.Excepciones;
using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiLectura.Models;

namespace WebApiLectura.Controllers;

// Solo rol Lector.
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Lector")]
public class LecturaController : ControllerBase
{
    private readonly IIniciarLectura iniciarLectura;
    private readonly IAvanzarLectura avanzarLectura;
    private readonly IObtenerLecturaPorId obtenerLecturaPorId;
    private readonly IEncontrarHistorialLecturas encontrarHistorialLecturas;

    public LecturaController(
        IIniciarLectura iniciarLecturaCu,
        IAvanzarLectura avanzarLecturaCu,
        IObtenerLecturaPorId obtenerLecturaPorIdCu,
        IEncontrarHistorialLecturas encontrarHistorialLecturasCu)
    {
        iniciarLectura = iniciarLecturaCu;
        avanzarLectura = avanzarLecturaCu;
        obtenerLecturaPorId = obtenerLecturaPorIdCu;
        encontrarHistorialLecturas = encontrarHistorialLecturasCu;
    }

    // El UsuarioId siempre sale del token, nunca del body/query.
    private int UsuarioId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>Devuelve una lectura del usuario logueado.</summary>
    /// <param name="id">Id de la lectura.</param>
    /// <response code="200">La lectura pedida.</response>
    /// <response code="403">La lectura es de otro usuario.</response>
    /// <response code="404">La lectura no existe.</response>
    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public ActionResult<LecturaDTO> Get(int id)
    {
        try
        {
            LecturaDTO lectura = obtenerLecturaPorId.Ejecutar(id);
            if (lectura.UsuarioId != UsuarioId)
                return StatusCode(403, "Esta lectura no te pertenece.");

            return Ok(lectura);
        }
        catch (LecturaNoEncontradaException lnex)
        {
            return NotFound(lnex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Error inesperado, vuelva a intentar más tarde.");
        }
    }

    /// <summary>Historial de lecturas del usuario logueado en un mes y año (RF08).</summary>
    /// <param name="mes">Mes (1 a 12).</param>
    /// <param name="anio">Año.</param>
    /// <response code="200">Lecturas del período; lista vacía si no hay.</response>
    [HttpGet("Historial")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public ActionResult<List<LecturaDTO>> Historial([FromQuery] int mes, [FromQuery] int anio)
    {
        try
        {
            return Ok(encontrarHistorialLecturas.Ejecutar(UsuarioId, mes, anio));
        }
        catch (Exception)
        {
            return StatusCode(500, "Error inesperado, vuelva a intentar más tarde.");
        }
    }

    /// <summary>Inicia una lectura nueva de una historia publicada, en su capítulo inicial (RF07).</summary>
    /// <response code="201">Lectura creada; el encabezado Location apunta a ella.</response>
    /// <response code="400">La historia no está publicada o no tiene capítulo inicial.</response>
    /// <response code="404">La historia no existe.</response>
    [HttpPost("Iniciar")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public ActionResult<LecturaDTO> Iniciar(IniciarLecturaRequestDTO request)
    {
        try
        {
            LecturaDTO lectura = iniciarLectura.Ejecutar(UsuarioId, request.HistoriaId);
            return CreatedAtAction(nameof(Get), new { id = lectura.Id }, lectura);
        }
        catch (HistoriaNoEncontradaException hnex)
        {
            return NotFound(hnex.Message);
        }
        catch (LecturaException lex)
        {
            return BadRequest(lex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Error inesperado, vuelva a intentar más tarde.");
        }
    }

    /// <summary>Avanza la lectura eligiendo una opción del capítulo actual; si llega a un final, la cierra (RF07).</summary>
    /// <param name="id">Id de la lectura.</param>
    /// <response code="200">Lectura actualizada.</response>
    /// <response code="400">La opción no es del capítulo actual o la lectura ya terminó.</response>
    /// <response code="403">La lectura es de otro usuario.</response>
    /// <response code="404">La lectura no existe.</response>
    [HttpPost("{id}/Avanzar")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public ActionResult<LecturaDTO> Avanzar(int id, AvanzarLecturaRequestDTO request)
    {
        try
        {
            return Ok(avanzarLectura.Ejecutar(id, request.OpcionId, UsuarioId));
        }
        catch (LecturaNoEncontradaException lnex)
        {
            return NotFound(lnex.Message);
        }
        catch (LecturaAjenaException laex)
        {
            return StatusCode(403, laex.Message);
        }
        catch (LecturaException lex)
        {
            return BadRequest(lex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Error inesperado, vuelva a intentar más tarde.");
        }
    }
}
