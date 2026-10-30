using System.Net;
using Dominio.Excepciones;
using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Historias;
using LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLectura.Controllers;

// Rankings y metricas de lectura: solo rol Administrador.
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Administrador")]
public class EstadisticaController : ControllerBase
{
    private readonly IEncontrarRankingHistorias encontrarRankingHistorias;
    private readonly IEncontrarAlcancesDeFinales encontrarAlcancesDeFinales;
    private readonly IEncontrarRankingUsuarios encontrarRankingUsuarios;

    public EstadisticaController(
        IEncontrarRankingHistorias encontrarRankingHistoriasCu,
        IEncontrarAlcancesDeFinales encontrarAlcancesDeFinalesCu,
        IEncontrarRankingUsuarios encontrarRankingUsuariosCu)
    {
        encontrarRankingHistorias = encontrarRankingHistoriasCu;
        encontrarAlcancesDeFinales = encontrarAlcancesDeFinalesCu;
        encontrarRankingUsuarios = encontrarRankingUsuariosCu;
    }

    // GET: api/Estadistica/HistoriasMasLeidas -- lista vacia (200) si no hay lecturas completadas.
    [HttpGet("HistoriasMasLeidas")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public ActionResult<List<RankingHistoriaDTO>> HistoriasMasLeidas()
    {
        try
        {
            return Ok(encontrarRankingHistorias.Ejecutar());
        }
        catch (Exception)
        {
            return StatusCode(500, "Error inesperado, vuelva a intentar más tarde.");
        }
    }

    // GET: api/Estadistica/FinalesPorHistoria/9
    [HttpGet("FinalesPorHistoria/{historiaId}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public ActionResult<List<AlcanceFinalDTO>> FinalesPorHistoria(int historiaId)
    {
        try
        {
            return Ok(encontrarAlcancesDeFinales.Ejecutar(historiaId));
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

    // GET: api/Estadistica/UsuariosConMasHistorias -- lista vacia (200) si nadie completo ninguna.
    [HttpGet("UsuariosConMasHistorias")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public ActionResult<List<RankingUsuarioDTO>> UsuariosConMasHistorias()
    {
        try
        {
            return Ok(encontrarRankingUsuarios.Ejecutar());
        }
        catch (Exception)
        {
            return StatusCode(500, "Error inesperado, vuelva a intentar más tarde.");
        }
    }
}
