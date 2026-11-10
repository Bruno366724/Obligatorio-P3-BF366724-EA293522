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

    /// <summary>Ranking de historias por cantidad de lecturas completadas (RF09).</summary>
    /// <response code="200">Ranking de mayor a menor; lista vacía si no hay lecturas completadas.</response>
    /// <response code="403">El usuario no es Administrador.</response>
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

    /// <summary>Cantidad de veces que se alcanzó cada final de una historia, marcando el más frecuente (RF10).</summary>
    /// <param name="historiaId">Id de la historia.</param>
    /// <response code="200">Finales de la historia con su contador.</response>
    /// <response code="403">El usuario no es Administrador.</response>
    /// <response code="404">La historia no existe.</response>
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
        catch (HistoriaNoEncontradaException hnex)
        {
            return NotFound(hnex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Error inesperado, vuelva a intentar más tarde.");
        }
    }

    /// <summary>Ranking de usuarios por cantidad de historias distintas completadas (RF11).</summary>
    /// <response code="200">Ranking de mayor a menor; lista vacía si nadie completó ninguna.</response>
    /// <response code="403">El usuario no es Administrador.</response>
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
