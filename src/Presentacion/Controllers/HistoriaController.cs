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

    // GET: HistoriaController
    public ActionResult Index()
    {
        return View(encontrarTodasHistorias.Ejecutar());
    }

    // GET: HistoriaController/Details/5
    public ActionResult Details(int id)
    {
        return View(obtenerHistoriaPorId.Ejecutar(id));
    }

    // GET: HistoriaController/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: HistoriaController/Create
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

    // GET: HistoriaController/Edit/5
    public ActionResult Edit(int id)
    {
        return View();
    }

    // POST: HistoriaController/Edit/5
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

    // GET: HistoriaController/Delete/5
    public ActionResult Delete(int id)
    {
        return View();
    }

    // POST: HistoriaController/Delete/5
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
