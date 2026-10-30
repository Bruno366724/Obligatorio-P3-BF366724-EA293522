using DTOs.DTOs;
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

    public EstadisticaController(IEncontrarRankingHistorias encontrarRankingHistoriasCu)
    {
        encontrarRankingHistorias = encontrarRankingHistoriasCu;
    }

    // GET: api/Estadistica/HistoriasMasLeidas -- lista vacia (200) si no hay lecturas completadas.
    [HttpGet("HistoriasMasLeidas")]
    public ActionResult<List<RankingHistoriaDTO>> HistoriasMasLeidas()
    {
        return Ok(encontrarRankingHistorias.Ejecutar());
    }
}
