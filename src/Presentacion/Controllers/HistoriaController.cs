using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Historias;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers;

public class HistoriaController : Controller
{
    private readonly IEncontrarTodasHistorias encontrarTodasHistorias;
    private readonly IAgregarHistoria agregarHistoria;
    private readonly IObtenerHistoriaPorId obtenerHistoriaPorId;

    public HistoriaController(
        IAgregarHistoria agregarHistoriaCu,
        IObtenerHistoriaPorId obtenerHistoriaPorIdCu,
        IEncontrarTodasHistorias encontrarTodasHistoriasCu)
    {
        agregarHistoria = agregarHistoriaCu;
        obtenerHistoriaPorId = obtenerHistoriaPorIdCu;
        encontrarTodasHistorias = encontrarTodasHistoriasCu;
    }

    public ActionResult Index()
    {
        return View(encontrarTodasHistorias.Ejecutar());
    }

    public ActionResult Details(int id)
    {
        return View(obtenerHistoriaPorId.Ejecutar(id));
    }

    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(HistoriaDTO historia)
    {
        try
        {
            agregarHistoria.Ejecutar(historia);
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
