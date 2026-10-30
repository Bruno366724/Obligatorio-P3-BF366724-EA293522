using System.Net.Http.Json;
using DTOs.DTOs;
using Microsoft.AspNetCore.Mvc;
using PresentacionUsuario.Models;

namespace PresentacionUsuario.Controllers;

// Login: delega la validacion en la Web API y guarda el token en Session.
public class AuthController : Controller
{
    private readonly IHttpClientFactory httpClientFactory;

    public AuthController(IHttpClientFactory clientFactory)
    {
        httpClientFactory = clientFactory;
    }

    public ActionResult Index()
    {
        return RedirectToAction("Index", "Historia");
    }

    // GET: /Auth/Login
    public ActionResult Login()
    {
        return View();
    }

    // POST: /Auth/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Login(LoginDTO dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        try
        {
            HttpClient cliente = httpClientFactory.CreateClient("WebApiLectura");

            HttpResponseMessage respuesta = await cliente.PostAsJsonAsync("api/Auth/login", dto);

            if (!respuesta.IsSuccessStatusCode)
            {
                // La Web API devuelve el mensaje de error en texto plano.
                string mensaje = await respuesta.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, string.IsNullOrWhiteSpace(mensaje) ? "Usuario o contraseña incorrectos." : mensaje);
                return View(dto);
            }

            LoginResponseDTO? datos = await respuesta.Content.ReadFromJsonAsync<LoginResponseDTO>();
            if (datos is null)
            {
                ModelState.AddModelError(string.Empty, "No se pudo iniciar sesión.");
                return View(dto);
            }

            HttpContext.Session.SetString("Token", datos.Token);
            HttpContext.Session.SetInt32("UsuarioId", datos.Usuario.Id);
            HttpContext.Session.SetString("NombreUsuario", datos.Usuario.NombreUsuario);
            HttpContext.Session.SetString("Rol", datos.Usuario.Rol.ToString());

            return RedirectToAction(nameof(Index));
        }
        catch
        {
            ModelState.AddModelError(string.Empty, "No se pudo conectar con el servidor, vuelva a intentar más tarde.");
            return View(dto);
        }
    }


    public ActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction(nameof(Login));
    }
}
