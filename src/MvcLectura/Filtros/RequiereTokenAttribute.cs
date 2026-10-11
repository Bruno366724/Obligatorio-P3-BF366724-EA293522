using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MvcLectura.Filtros;

// Protege rutas que requieren tener el token de la Web API guardado en Session.
public class RequiereTokenAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        string? token = context.HttpContext.Session.GetString("Token");
        if (string.IsNullOrEmpty(token))
        {
            context.Result = new RedirectToActionResult("Login", "Cuenta", null);
            return;
        }

        base.OnActionExecuting(context);
    }
}
