using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Categorias;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers;

public class CategoriaController : Controller
{
    private readonly IEncontrarTodasCategorias encontrarTodasCategorias;
    private readonly IAgregarCategoria agregarCategoria;
    private readonly IObtenerCategoriaPorId obtenerCategoriaPorId;

    public CategoriaController(
        IAgregarCategoria agregarCategoriaCu,
        IObtenerCategoriaPorId obtenerCategoriaPorIdCu,
        IEncontrarTodasCategorias encontrarTodasCategoriasCu)
    {
        agregarCategoria = agregarCategoriaCu;
        obtenerCategoriaPorId = obtenerCategoriaPorIdCu;
        encontrarTodasCategorias = encontrarTodasCategoriasCu;
    }

    public ActionResult Index()
    {
        return View(encontrarTodasCategorias.Ejecutar());
    }

    public ActionResult Details(int id)
    {
        return View(obtenerCategoriaPorId.Ejecutar(id));
    }

    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(CategoriaDTO categoria)
    {
        try
        {
            agregarCategoria.Ejecutar(categoria);
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    public ActionResult Edit(int id)
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    public ActionResult Delete(int id)
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }
}
