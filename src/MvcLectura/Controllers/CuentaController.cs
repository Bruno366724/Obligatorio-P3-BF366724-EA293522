using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using MvcLectura.Models;

namespace MvcLectura.Controllers;

// Login: delega la validacion en la Web API y guarda el token en Session.
public class CuentaController : Controller
{
    private readonly IHttpClientFactory httpClientFactory;

    public CuentaController(IHttpClientFactory clientFactory)
    {
        httpClientFactory = clientFactory;
    }

    public ActionResult Index()
    {
        return RedirectToAction("Index", "Historia");
    }

    public ActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Login(string nombreUsuario, string password)
    {
        HttpClient cliente = httpClientFactory.CreateClient("WebApiLectura");

        HttpResponseMessage respuesta = await cliente.PostAsJsonAsync("api/Autenticacion/Login",
            new LoginRequestDTO { NombreUsuario = nombreUsuario, Password = password });

        if (!respuesta.IsSuccessStatusCode)
        {
            // La Web API devuelve 401 con el mensaje en texto plano.
            string mensaje = await respuesta.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, string.IsNullOrWhiteSpace(mensaje) ? "Usuario o contraseña incorrectos." : mensaje);
            return View();
        }

        LoginResponseDTO? datos = await respuesta.Content.ReadFromJsonAsync<LoginResponseDTO>();
        if (datos is null)
        {
            ModelState.AddModelError(string.Empty, "No se pudo iniciar sesión.");
            return View();
        }

        HttpContext.Session.SetString("Token", datos.Token);
        HttpContext.Session.SetInt32("UsuarioId", datos.Id);
        HttpContext.Session.SetString("NombreUsuario", datos.NombreUsuario);
        HttpContext.Session.SetString("Rol", datos.Rol.ToString());

        return RedirectToAction(nameof(Index));
    }

    public ActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction(nameof(Login));
    }
}
