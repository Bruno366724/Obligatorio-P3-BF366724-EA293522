using System.Net;
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

    // GET: api/Historia?categoriaId=5 -- solo Publicadas, categoria opcional.
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

    // GET: api/Historia/5
    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public ActionResult<HistoriaDTO> Get(int id)
    {
        try
        {
            return Ok(obtenerHistoriaPorId.Ejecutar(id));
        }
        catch (HistoriaException hex)
        {
            return NotFound(hex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Error inesperado, vuelva a intentar más tarde.");
        }
    }
}
