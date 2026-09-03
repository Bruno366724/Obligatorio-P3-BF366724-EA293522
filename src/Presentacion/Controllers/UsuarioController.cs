using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers;

public class UsuarioController : Controller
{
    private readonly IEncontrarTodosUsuarios encontrarTodosUsuarios;
    private readonly IAgregarUsuario agregarUsuario;
    private readonly IObtenerUsuarioPorId obtenerUsuarioPorId;

    public UsuarioController(
        IAgregarUsuario agregarUsuarioCu,
        IObtenerUsuarioPorId obtenerUsuarioPorIdCu,
        IEncontrarTodosUsuarios encontrarTodosUsuariosCu)
    {
        agregarUsuario = agregarUsuarioCu;
        obtenerUsuarioPorId = obtenerUsuarioPorIdCu;
        encontrarTodosUsuarios = encontrarTodosUsuariosCu;
    }

    // GET: UsuarioController
    public ActionResult Index()
    {
        return View(encontrarTodosUsuarios.Ejecutar());
    }

    // GET: UsuarioController/Details/5
    public ActionResult Details(int id)
    {
        return View(obtenerUsuarioPorId.Ejecutar(id));
    }

    // GET: UsuarioController/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: UsuarioController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(UsuarioDTO usuario)
    {
        try
        {
            agregarUsuario.Ejecutar(usuario);
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    // GET: UsuarioController/Edit/5
    public ActionResult Edit(int id)
    {
        return View();
    }

    // POST: UsuarioController/Edit/5
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

    // GET: UsuarioController/Delete/5
    public ActionResult Delete(int id)
    {
        return View();
    }

    // POST: UsuarioController/Delete/5
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
