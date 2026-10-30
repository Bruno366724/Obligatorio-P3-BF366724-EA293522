using Microsoft.AspNetCore.Http;

namespace PresentacionUsuario.Estilos;

// Estilo de lectura (RF12): oscuro por defecto, claro si el lector lo eligio (cookie por usuario).
public static class PreferenciaEstilo
{
    public const string Claro = "claro";
    public const string Oscuro = "oscuro";

    public static string NombreCookie(string nombreUsuario) => $"Estilo_{nombreUsuario}";

    // Solo un Lector puede tener el estilo claro; cualquier otro caso es oscuro.
    public static bool EsClaro(HttpContext contexto)
    {
        string? rol = contexto.Session.GetString("Rol");
        string? nombreUsuario = contexto.Session.GetString("NombreUsuario");
        if (rol != "Lector" || string.IsNullOrEmpty(nombreUsuario))
            return false;

        return contexto.Request.Cookies[NombreCookie(nombreUsuario)] == Claro;
    }
}
