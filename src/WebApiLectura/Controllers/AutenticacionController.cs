using Dominio.Excepciones;
using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios;
using Microsoft.AspNetCore.Mvc;
using WebApiLectura.Models;
using WebApiLectura.Servicios;

namespace WebApiLectura.Controllers;

// Login de la app de Lectura (Administradores y Lectores).
[Route("api/[controller]")]
[ApiController]
public class AutenticacionController : ControllerBase
{
    private readonly IIniciarSesionLectura iniciarSesion;
    private readonly GeneradorTokenJwt generadorToken;

    public AutenticacionController(IIniciarSesionLectura iniciarSesionCu, GeneradorTokenJwt generadorTokenJwt)
    {
        iniciarSesion = iniciarSesionCu;
        generadorToken = generadorTokenJwt;
    }

    // POST: api/Autenticacion/Login
    [HttpPost("Login")]
    public ActionResult Login(LoginRequestDTO request)
    {
        try
        {
            UsuarioDTO usuario = iniciarSesion.Ejecutar(request.NombreUsuario, request.Password);
            string token = generadorToken.Generar(usuario);

            return Ok(new
            {
                token,
                usuario.Id,
                usuario.NombreUsuario,
                usuario.Rol
            });
        }
        catch (DomainException ex)
        {
            // 401: credencial invalida.
            return Unauthorized(ex.Message);
        }
    }
}
