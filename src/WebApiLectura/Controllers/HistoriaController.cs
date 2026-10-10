using Dominio.Excepciones;
using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Historias;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLectura.Controllers;

// Solo lectura por ahora (GET) -- RF06+ (listar/leer historias publicadas, avanzar
// por las opciones de cada capitulo) todavia no esta implementado como tal, esto es
// la base minima para no arrancar de cero cuando se ataque esa parte.
[Route("api/[controller]")]
[ApiController]
public class HistoriaController : ControllerBase
{
    private readonly IEncontrarTodasHistorias encontrarTodasHistorias;
    private readonly IObtenerHistoriaPorId obtenerHistoriaPorId;

    public HistoriaController(
        IEncontrarTodasHistorias encontrarTodasHistoriasCu,
        IObtenerHistoriaPorId obtenerHistoriaPorIdCu)
    {
        encontrarTodasHistorias = encontrarTodasHistoriasCu;
        obtenerHistoriaPorId = obtenerHistoriaPorIdCu;
    }

    // GET: api/Historia
    [HttpGet]
    public ActionResult<List<HistoriaDTO>> Get()
    {
        return Ok(encontrarTodasHistorias.Ejecutar());
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
