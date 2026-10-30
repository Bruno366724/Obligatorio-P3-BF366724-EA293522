using System.Net.Http.Json;
using DTOs.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace PresentacionUsuario.Controllers;

// Listado de historias publicadas (filtrable por categoria) + detalle.
public class HistoriaController : ControladorBase
{
    public HistoriaController(IHttpClientFactory clientFactory) : base(clientFactory)
    {
    }

    public async Task<ActionResult> Index(int? categoriaId)
    {
        ViewBag.CategoriaIdSeleccionada = categoriaId;
        try
        {
            HttpClient cliente = CrearClienteAutenticado();

            string url = categoriaId.HasValue ? $"api/Historia?categoriaId={categoriaId}" : "api/Historia";
            List<HistoriaDTO> historias = await cliente.GetFromJsonAsync<List<HistoriaDTO>>(url) ?? [];
            ViewBag.Categorias = await cliente.GetFromJsonAsync<List<CategoriaDTO>>("api/Categoria") ?? [];

            return View(historias);
        }
        catch
        {
            ViewBag.Error = ErrorConexion;
            ViewBag.Categorias = new List<CategoriaDTO>();
            return View(new List<HistoriaDTO>());
        }
    }

    public async Task<ActionResult> Details(int id)
    {
        try
        {
            HttpClient cliente = CrearClienteAutenticado();

            HttpResponseMessage respuesta = await cliente.GetAsync($"api/Historia/{id}");
            if (!respuesta.IsSuccessStatusCode)
            {
                TempData["Error"] = await respuesta.Content.ReadAsStringAsync();
                return RedirectToAction(nameof(Index));
            }

            HistoriaDTO? historia = await respuesta.Content.ReadFromJsonAsync<HistoriaDTO>();
            return View(historia);
        }
        catch
        {
            TempData["Error"] = ErrorConexion;
            return RedirectToAction(nameof(Index));
        }
    }
}
