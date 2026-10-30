using Dominio.Excepciones;
using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PresentacionAdmin.Filtros;

namespace PresentacionAdmin.Controllers;

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
        catch (UsuarioException uex)
        {
            ModelState.AddModelError(string.Empty, uex.Message);
            return View();
        }
        catch
        {
            ModelState.AddModelError(string.Empty, "Error en el proceso de autenticación.");
            return View();
        }
    }

    public ActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction(nameof(Login));
    }

    [RequiereSesion]
    public ActionResult Index()
    {
        return View(encontrarTodosUsuarios.Ejecutar());
    }

    [RequiereSesion]
    public ActionResult Details(int id)
    {
        try
        {
            return View(obtenerUsuarioPorId.Ejecutar(id));
        }
        catch (UsuarioException uex)
        {
            TempData["Error"] = uex.Message;
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            TempData["Error"] = "Error inesperado al cargar el usuario.";
            return RedirectToAction(nameof(Index));
        }
    }

    [RequiereSesion]
    public ActionResult Create()
    {
        return View();
    }

    [RequiereSesion]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(UsuarioDTO usuario)
    {
        try
        {
            agregarUsuario.Ejecutar(usuario);
            return RedirectToAction(nameof(Index));
        }
        catch (UsuarioException uex)
        {
            ModelState.AddModelError(string.Empty, uex.Message);
            return View(usuario);
        }
        catch
        {
            ModelState.AddModelError(string.Empty, "Error inesperado al crear el usuario, vuelva a intentar.");
            return View(usuario);
        }
    }

    [RequiereSesion]
    public ActionResult Edit(int id)
    {
        return View();
    }

    [RequiereSesion]
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

    [RequiereSesion]
    public ActionResult Delete(int id)
    {
        return View();
    }

    [RequiereSesion]
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
