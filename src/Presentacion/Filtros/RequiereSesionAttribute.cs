using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Presentacion.Filtros;

// Protege rutas que requieren haber iniciado sesion (chequeo manual, sin [Authorize]).
public class RequiereSesionAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        int? usuarioId = context.HttpContext.Session.GetInt32("UsuarioId");
        if (usuarioId is null)
        {
            context.Result = new RedirectToActionResult("Login", "Usuario", null);
            return;
        }

        base.OnActionExecuting(context);
    }
}
