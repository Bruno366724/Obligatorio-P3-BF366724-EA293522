using System.Net.Http.Json;
using DTOs.DTOs;
using Microsoft.AspNetCore.Mvc;
using PresentacionUsuario.Models;

namespace PresentacionUsuario.Controllers;

// Flujo de lectura: iniciar, mostrar el capitulo actual, avanzar.
public class LecturaController : ControladorBase
{
    public LecturaController(IHttpClientFactory clientFactory) : base(clientFactory)
    {
    }

    // POST: Lectura/Iniciar
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Iniciar(int historiaId)
    {
        try
        {
            HttpClient cliente = CrearClienteAutenticado();

            HttpResponseMessage respuesta = await cliente.PostAsJsonAsync("api/Lectura/Iniciar", new { historiaId });
            if (!respuesta.IsSuccessStatusCode)
            {
                TempData["Error"] = await respuesta.Content.ReadAsStringAsync();
                return RedirectToAction("Details", "Historia", new { id = historiaId });
            }

            LecturaDTO? lectura = await respuesta.Content.ReadFromJsonAsync<LecturaDTO>();
            return RedirectToAction(nameof(Leer), new { id = lectura!.Id });
        }
        catch
        {
            TempData["Error"] = ErrorConexion;
            return RedirectToAction("Details", "Historia", new { id = historiaId });
        }
    }

    // GET: Lectura/Leer/5
    public async Task<ActionResult> Leer(int id)
    {
        try
        {
            HttpClient cliente = CrearClienteAutenticado();

            HttpResponseMessage respuestaLectura = await cliente.GetAsync($"api/Lectura/{id}");
            if (!respuestaLectura.IsSuccessStatusCode)
            {
                TempData["Error"] = "No se encontró la lectura solicitada.";
                return RedirectToAction("Index", "Historia");
            }
            LecturaDTO lectura = (await respuestaLectura.Content.ReadFromJsonAsync<LecturaDTO>())!;

            HttpResponseMessage respuestaHistoria = await cliente.GetAsync($"api/Historia/{lectura.HistoriaId}");
            if (!respuestaHistoria.IsSuccessStatusCode)
            {
                TempData["Error"] = await respuestaHistoria.Content.ReadAsStringAsync();
                return RedirectToAction("Index", "Historia");
            }
            HistoriaDTO historia = (await respuestaHistoria.Content.ReadFromJsonAsync<HistoriaDTO>())!;

            CapituloDTO capituloActual = historia.Capitulos.First(capitulo => capitulo.Id == lectura.CapituloActualId);

            return View(new LeerViewModel
            {
                Lectura = lectura,
                Historia = historia,
                CapituloActual = capituloActual
            });
        }
        catch
        {
            TempData["Error"] = ErrorConexion;
            return RedirectToAction("Index", "Historia");
        }
    }

    // POST: Lectura/Avanzar/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Avanzar(int id, int opcionId)
    {
        try
        {
            HttpClient cliente = CrearClienteAutenticado();

            HttpResponseMessage respuesta = await cliente.PostAsJsonAsync($"api/Lectura/{id}/Avanzar", new { opcionId });
            if (!respuesta.IsSuccessStatusCode)
                TempData["Error"] = await respuesta.Content.ReadAsStringAsync();
        }
        catch
        {
            TempData["Error"] = ErrorConexion;
        }

        return RedirectToAction(nameof(Leer), new { id });
    }

    // GET: Lectura/Historial?mes=10&anio=2026 -- sin parametros, usa el mes actual.
    public async Task<ActionResult> Historial(int? mes, int? anio)
    {
        int mesEfectivo = mes ?? DateTime.Now.Month;
        int anioEfectivo = anio ?? DateTime.Now.Year;
        ViewBag.Mes = mesEfectivo;
        ViewBag.Anio = anioEfectivo;

        try
        {
            HttpClient cliente = CrearClienteAutenticado();

            List<LecturaDTO> lecturas = await cliente.GetFromJsonAsync<List<LecturaDTO>>(
                $"api/Lectura/Historial?mes={mesEfectivo}&anio={anioEfectivo}") ?? [];

            // Pide cada Historia para mostrar nombres en vez de ids.
            List<HistorialItemViewModel> items = [];
            foreach (LecturaDTO lectura in lecturas)
            {
                HistoriaDTO? historia = await cliente.GetFromJsonAsync<HistoriaDTO>($"api/Historia/{lectura.HistoriaId}");
                string? finalTitulo = lectura.FinalAlcanzadoId.HasValue
                    ? historia?.Capitulos.FirstOrDefault(c => c.Id == lectura.FinalAlcanzadoId)?.Titulo
                    : null;

                items.Add(new HistorialItemViewModel
                {
                    Lectura = lectura,
                    HistoriaTitulo = historia?.Titulo ?? "(historia no encontrada)",
                    FinalTitulo = finalTitulo
                });
            }

            return View(items);
        }
        catch
        {
            ViewBag.Error = ErrorConexion;
            return View(new List<HistorialItemViewModel>());
        }
    }
}
