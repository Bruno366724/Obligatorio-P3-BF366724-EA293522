using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using PresentacionUsuario.Filtros;

namespace PresentacionUsuario.Controllers;

// Base para los controllers que hablan con la Web API.
[RequiereToken]
public abstract class ControladorBase : Controller
{
    protected const string ErrorConexion = "No se pudo conectar con el servidor, vuelva a intentar más tarde.";

    private readonly IHttpClientFactory httpClientFactory;

    protected ControladorBase(IHttpClientFactory clientFactory)
    {
        httpClientFactory = clientFactory;
    }

    // HttpClient con el token de Session en el header Authorization.
    protected HttpClient CrearClienteAutenticado()
    {
        HttpClient cliente = httpClientFactory.CreateClient("WebApiLectura");
        string token = HttpContext.Session.GetString("Token")!;
        cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return cliente;
    }
}
