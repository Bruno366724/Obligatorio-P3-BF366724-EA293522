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
    public ActionResult<List<RankingHistoriaDTO>> HistoriasMasLeidas()
    {
        return Ok(encontrarRankingHistorias.Ejecutar());
    }

    // GET: api/Estadistica/FinalesPorHistoria/9
    [HttpGet("FinalesPorHistoria/{historiaId}")]
    public ActionResult<List<AlcanceFinalDTO>> FinalesPorHistoria(int historiaId)
    {
        try
        {
            return Ok(encontrarAlcancesDeFinales.Ejecutar(historiaId));
        }
        catch (DomainException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // GET: api/Estadistica/UsuariosConMasHistorias -- lista vacia (200) si nadie completo ninguna.
    [HttpGet("UsuariosConMasHistorias")]
    public ActionResult<List<RankingUsuarioDTO>> UsuariosConMasHistorias()
    {
        return Ok(encontrarRankingUsuarios.Ejecutar());
    }
}
