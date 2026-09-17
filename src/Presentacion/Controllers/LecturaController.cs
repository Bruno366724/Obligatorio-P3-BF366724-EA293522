using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers;

public class LecturaController : Controller
{
    private readonly IEncontrarTodasLecturas encontrarTodasLecturas;
    private readonly IAgregarLectura agregarLectura;
    private readonly IObtenerLecturaPorId obtenerLecturaPorId;

    public LecturaController(
        IAgregarLectura agregarLecturaCu,
        IObtenerLecturaPorId obtenerLecturaPorIdCu,
        IEncontrarTodasLecturas encontrarTodasLecturasCu)
    {
        agregarLectura = agregarLecturaCu;
        obtenerLecturaPorId = obtenerLecturaPorIdCu;
        encontrarTodasLecturas = encontrarTodasLecturasCu;
    }

    public ActionResult Index()
    {
        return View(encontrarTodasLecturas.Ejecutar());
    }

    public ActionResult Details(int id)
    {
        return View(obtenerLecturaPorId.Ejecutar(id));
    }

    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(LecturaDTO lectura)
    {
        try
        {
            agregarLectura.Ejecutar(lectura);
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
