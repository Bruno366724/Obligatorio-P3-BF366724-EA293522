using System.Net;
using Dominio.Enumerados;
using Dominio.Excepciones;
using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Historias;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLectura.Controllers;

// Requiere JWT valido (cualquier rol).
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class HistoriaController : ControllerBase
{
    private readonly IEncontrarHistoriasPublicadas encontrarHistoriasPublicadas;
    private readonly IObtenerHistoriaPorId obtenerHistoriaPorId;

    public HistoriaController(
        IEncontrarHistoriasPublicadas encontrarHistoriasPublicadasCu,
        IObtenerHistoriaPorId obtenerHistoriaPorIdCu)
    {
        encontrarHistoriasPublicadas = encontrarHistoriasPublicadasCu;
        obtenerHistoriaPorId = obtenerHistoriaPorIdCu;
    }

    /// <summary>Lista las historias publicadas, con filtro opcional por categoría (RF07).</summary>
    /// <param name="categoriaId">Id de la categoría (opcional).</param>
    /// <response code="200">Historias publicadas.</response>
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public ActionResult<List<HistoriaDTO>> Get([FromQuery] int? categoriaId)
    {
        try
        {
            return Ok(encontrarHistoriasPublicadas.Ejecutar(categoriaId));
        }
        catch (Exception)
        {
            return StatusCode(500, "Error inesperado, vuelva a intentar más tarde.");
        }
    }

    /// <summary>Devuelve una historia con sus capítulos y opciones.</summary>
    /// <param name="id">Id de la historia.</param>
    /// <response code="200">La historia pedida.</response>
    /// <response code="404">La historia no existe, o el usuario es Lector y la historia no está publicada.</response>
    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public ActionResult<HistoriaDTO> Get(int id)
    {
        try
        {
            HistoriaDTO historia = obtenerHistoriaPorId.Ejecutar(id);

            // Un Lector solo ve historias publicadas: un borrador es como si no existiera.
            if (User.IsInRole("Lector") && historia.Estado != EstadoHistoria.Publicada)
                return NotFound("No se encontró la historia solicitada.");

            return Ok(historia);
        }
        catch (HistoriaNoEncontradaException hnex)
        {
            return NotFound(hnex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Error inesperado, vuelva a intentar más tarde.");
        }
    }
}
