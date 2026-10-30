using Microsoft.AspNetCore.Mvc;
using PresentacionUsuario.Estilos;
using PresentacionUsuario.Filtros;

namespace PresentacionUsuario.Controllers;

// RF12: el Lector alterna entre estilo oscuro (base) y claro.
[RequiereToken]
public class EstiloController : Controller
{
    // POST: Estilo/Cambiar
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Cambiar(string? returnUrl)
    {
        if (HttpContext.Session.GetString("Rol") != "Lector")
            return View("SinPermiso");

        string nombreUsuario = HttpContext.Session.GetString("NombreUsuario")!;
        string nuevoEstilo = PreferenciaEstilo.EsClaro(HttpContext) ? PreferenciaEstilo.Oscuro : PreferenciaEstilo.Claro;

        Response.Cookies.Append(PreferenciaEstilo.NombreCookie(nombreUsuario), nuevoEstilo, new CookieOptions
        {
            Expires = DateTimeOffset.Now.AddYears(1),
            HttpOnly = true,
            IsEssential = true,
            SameSite = SameSiteMode.Lax
        });

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return LocalRedirect(returnUrl);

        return RedirectToAction("Index", "Historia");
    }
}
