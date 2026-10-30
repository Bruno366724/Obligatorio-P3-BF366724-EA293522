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
        try
        {
            HttpClient cliente = CrearClienteAutenticado();

            HttpResponseMessage respuesta = await cliente.GetAsync("api/Estadistica/HistoriasMasLeidas");
            if (respuesta.StatusCode == HttpStatusCode.Forbidden)
                return View("SinPermiso");
            if (!respuesta.IsSuccessStatusCode)
            {
                ViewBag.Error = await respuesta.Content.ReadAsStringAsync();
                return View(new List<RankingHistoriaDTO>());
            }

            List<RankingHistoriaDTO> ranking = await respuesta.Content.ReadFromJsonAsync<List<RankingHistoriaDTO>>() ?? [];
            return View(ranking);
        }
        catch
        {
            ViewBag.Error = ErrorConexion;
            return View(new List<RankingHistoriaDTO>());
        }
    }

    // GET: Estadistica/UsuariosConMasHistorias
    public async Task<ActionResult> UsuariosConMasHistorias()
    {
        try
        {
            HttpClient cliente = CrearClienteAutenticado();

            HttpResponseMessage respuesta = await cliente.GetAsync("api/Estadistica/UsuariosConMasHistorias");
            if (respuesta.StatusCode == HttpStatusCode.Forbidden)
                return View("SinPermiso");
            if (!respuesta.IsSuccessStatusCode)
            {
                ViewBag.Error = await respuesta.Content.ReadAsStringAsync();
                return View(new List<RankingUsuarioDTO>());
            }

            List<RankingUsuarioDTO> ranking = await respuesta.Content.ReadFromJsonAsync<List<RankingUsuarioDTO>>() ?? [];
            return View(ranking);
        }
        catch
        {
            ViewBag.Error = ErrorConexion;
            return View(new List<RankingUsuarioDTO>());
        }
    }

    // GET: Estadistica/FinalesPorHistoria?historiaId=9 -- sin historiaId, solo muestra el combo.
    public async Task<ActionResult> FinalesPorHistoria(int? historiaId)
    {
        if (HttpContext.Session.GetString("Rol") != "Administrador")
            return View("SinPermiso");

        FinalesPorHistoriaViewModel modelo = new FinalesPorHistoriaViewModel { HistoriaId = historiaId };
        try
        {
            HttpClient cliente = CrearClienteAutenticado();

            modelo.Historias = await cliente.GetFromJsonAsync<List<HistoriaDTO>>("api/Historia") ?? [];

            if (historiaId.HasValue)
            {
                HttpResponseMessage respuesta = await cliente.GetAsync($"api/Estadistica/FinalesPorHistoria/{historiaId}");
                if (respuesta.StatusCode == HttpStatusCode.Forbidden)
                    return View("SinPermiso");

                if (respuesta.IsSuccessStatusCode)
                    modelo.Finales = await respuesta.Content.ReadFromJsonAsync<List<AlcanceFinalDTO>>() ?? [];
                else
                    ViewBag.Error = await respuesta.Content.ReadAsStringAsync();
            }
        }
        catch
        {
            ViewBag.Error = ErrorConexion;
        }

        return View(modelo);
    }
}
