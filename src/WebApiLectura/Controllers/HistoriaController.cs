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
    public ActionResult<List<HistoriaDTO>> Get([FromQuery] int? categoriaId)
    {
        return Ok(encontrarHistoriasPublicadas.Ejecutar(categoriaId));
    }

    // GET: api/Historia/5
    [HttpGet("{id}")]
    public ActionResult<HistoriaDTO> Get(int id)
    {
        try
        {
            return Ok(obtenerHistoriaPorId.Ejecutar(id));
        }
        catch (DomainException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
