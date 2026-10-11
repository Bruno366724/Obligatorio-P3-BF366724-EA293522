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

    // GET: api/Lectura/5 -- chequea pertenencia aca (el CU es generico, sin esa nocion).
    [HttpGet("{id}")]
    public ActionResult<LecturaDTO> Get(int id)
    {
        try
        {
            LecturaDTO lectura = obtenerLecturaPorId.Ejecutar(id);
            if (lectura.UsuarioId != UsuarioId)
                return Forbid();

            return Ok(lectura);
        }
        catch (DomainException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // GET: api/Lectura/Historial?mes=10&anio=2026 -- lista vacia (200) si no hay lecturas.
    [HttpGet("Historial")]
    public ActionResult<List<LecturaDTO>> Historial([FromQuery] int mes, [FromQuery] int anio)
    {
        return Ok(encontrarHistorialLecturas.Ejecutar(UsuarioId, mes, anio));
    }

    // POST: api/Lectura/Iniciar
    [HttpPost("Iniciar")]
    public ActionResult<LecturaDTO> Iniciar(IniciarLecturaRequestDTO request)
    {
        try
        {
            return Ok(iniciarLectura.Ejecutar(UsuarioId, request.HistoriaId));
        }
        catch (DomainException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // POST: api/Lectura/5/Avanzar
    [HttpPost("{id}/Avanzar")]
    public ActionResult<LecturaDTO> Avanzar(int id, AvanzarLecturaRequestDTO request)
    {
        try
        {
            return Ok(avanzarLectura.Ejecutar(id, request.OpcionId, UsuarioId));
        }
        catch (DomainException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
