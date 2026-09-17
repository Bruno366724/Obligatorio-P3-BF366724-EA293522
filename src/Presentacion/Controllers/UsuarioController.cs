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

    // GET: UsuarioController/Login
    public ActionResult Login()
    {
        return View();
    }

    // POST: UsuarioController/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Login(string nombreUsuario, string password)
    {
        try
        {
            var usuario = iniciarSesion.Ejecutar(nombreUsuario, password);

            // Guardamos en Session lo mínimo para saber "quién está logueado"
            // en los pedidos siguientes (no se guarda la contraseña).
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

    // GET: UsuarioController/Logout
    public ActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction(nameof(Login));
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
        catch (DomainException ex)
        {
            // CAMBIO: antes el catch descartaba el motivo del error; ahora se
            // muestra en la vista (regla general: informar al usuario en las ABM).
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(usuario);
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
