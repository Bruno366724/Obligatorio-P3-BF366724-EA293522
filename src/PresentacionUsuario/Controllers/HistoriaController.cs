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
        HttpClient cliente = CrearClienteAutenticado();

        string url = categoriaId.HasValue ? $"api/Historia?categoriaId={categoriaId}" : "api/Historia";
        List<HistoriaDTO> historias = await cliente.GetFromJsonAsync<List<HistoriaDTO>>(url) ?? [];
        List<CategoriaDTO> categorias = await cliente.GetFromJsonAsync<List<CategoriaDTO>>("api/Categoria") ?? [];

        ViewBag.Categorias = categorias;
        ViewBag.CategoriaIdSeleccionada = categoriaId;

        return View(historias);
    }

    public async Task<ActionResult> Details(int id)
    {
        HttpClient cliente = CrearClienteAutenticado();

        HttpResponseMessage respuesta = await cliente.GetAsync($"api/Historia/{id}");
        if (!respuesta.IsSuccessStatusCode)
            return NotFound();

        HistoriaDTO? historia = await respuesta.Content.ReadFromJsonAsync<HistoriaDTO>();
        return View(historia);
    }
}
