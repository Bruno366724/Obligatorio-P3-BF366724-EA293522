using Dominio.Excepciones;
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
    private readonly IIniciarSesion iniciarSesion;

    public UsuarioController(
        IAgregarUsuario agregarUsuarioCu,
        IObtenerUsuarioPorId obtenerUsuarioPorIdCu,
        IEncontrarTodosUsuarios encontrarTodosUsuariosCu,
        IIniciarSesion iniciarSesionCu)
    {
        agregarUsuario = agregarUsuarioCu;
        obtenerUsuarioPorId = obtenerUsuarioPorIdCu;
        encontrarTodosUsuarios = encontrarTodosUsuariosCu;
        iniciarSesion = iniciarSesionCu;
    }

    public ActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Login(string nombreUsuario, string password)
    {
        try
        {
            UsuarioDTO usuario = iniciarSesion.Ejecutar(nombreUsuario, password);

            HttpContext.Session.SetInt32("UsuarioId", usuario.Id);
            HttpContext.Session.SetString("NombreUsuario", usuario.NombreUsuario);
            HttpContext.Session.SetString("Rol", usuario.Rol.ToString());

            return RedirectToAction(nameof(Index));
        }
        catch (DomainException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    public ActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction(nameof(Login));
    }

    public ActionResult Index()
    {
        return View(encontrarTodosUsuarios.Ejecutar());
    }

    public ActionResult Details(int id)
    {
        return View(obtenerUsuarioPorId.Ejecutar(id));
    }

    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(UsuarioDTO usuario)
    {
        try
        {
            agregarUsuario.Ejecutar(usuario);
            return RedirectToAction(nameof(Index));
        }
        catch (DomainException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(usuario);
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
