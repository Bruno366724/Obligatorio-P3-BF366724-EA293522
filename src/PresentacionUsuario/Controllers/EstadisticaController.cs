using System.Net;
using System.Net.Http.Json;
using DTOs.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace PresentacionUsuario.Controllers;

// Pantallas de rankings y metricas del rol Administrador.
public class EstadisticaController : ControladorBase
{
    public EstadisticaController(IHttpClientFactory clientFactory) : base(clientFactory)
    {
    }

    // GET: Estadistica/HistoriasMasLeidas
    public async Task<ActionResult> HistoriasMasLeidas()
    {
        HttpClient cliente = CrearClienteAutenticado();

        HttpResponseMessage respuesta = await cliente.GetAsync("api/Estadistica/HistoriasMasLeidas");
        if (respuesta.StatusCode == HttpStatusCode.Forbidden)
            return View("SinPermiso");

        List<RankingHistoriaDTO> ranking = await respuesta.Content.ReadFromJsonAsync<List<RankingHistoriaDTO>>() ?? [];
        return View(ranking);
    }
}
