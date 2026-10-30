using System.Net;
using System.Net.Http.Json;
using DTOs.DTOs;
using Microsoft.AspNetCore.Mvc;
using PresentacionUsuario.Models;

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

    // GET: Estadistica/FinalesPorHistoria?historiaId=9 -- sin historiaId, solo muestra el combo.
    public async Task<ActionResult> FinalesPorHistoria(int? historiaId)
    {
        if (HttpContext.Session.GetString("Rol") != "Administrador")
            return View("SinPermiso");

        HttpClient cliente = CrearClienteAutenticado();

        FinalesPorHistoriaViewModel modelo = new FinalesPorHistoriaViewModel
        {
            Historias = await cliente.GetFromJsonAsync<List<HistoriaDTO>>("api/Historia") ?? [],
            HistoriaId = historiaId
        };

        if (historiaId.HasValue)
        {
            HttpResponseMessage respuesta = await cliente.GetAsync($"api/Estadistica/FinalesPorHistoria/{historiaId}");
            if (respuesta.StatusCode == HttpStatusCode.Forbidden)
                return View("SinPermiso");

            if (respuesta.IsSuccessStatusCode)
                modelo.Finales = await respuesta.Content.ReadFromJsonAsync<List<AlcanceFinalDTO>>() ?? [];
            else
                TempData["Error"] = await respuesta.Content.ReadAsStringAsync();
        }

        return View(modelo);
    }
}
