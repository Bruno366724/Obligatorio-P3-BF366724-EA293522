using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PresentacionUsuario.Filtros;

// Protege rutas que requieren tener el token de la Web API guardado en Session.
public class RequiereTokenAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        string? token = context.HttpContext.Session.GetString("Token");
        if (string.IsNullOrEmpty(token))
        {
            context.Result = new RedirectToActionResult("Login", "Auth", null);
            return;
        }

        base.OnActionExecuting(context);
    }
}
