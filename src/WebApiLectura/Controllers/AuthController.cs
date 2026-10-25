using System.Net;
using Dominio.Excepciones;
using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiLectura.TokenHandler;

namespace WebApiLectura.Controllers;

// Login de la app de Lectura (Administradores y Lectores).
[Route("api/[controller]")]
[AllowAnonymous]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IIniciarSesionLectura iniciarSesion;
    private readonly IConfiguration configuracion;

    public AuthController(IIniciarSesionLectura iniciarSesionCu, IConfiguration configuration)
    {
        iniciarSesion = iniciarSesionCu;
        configuracion = configuration;
    }

    // POST: api/Auth/login
    [HttpPost("login")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public ActionResult Login([FromBody] LoginDTO dto)
    {
        try
        {
            UsuarioDTO usuario = iniciarSesion.Ejecutar(dto.NombreUsuario, dto.Password);
            string token = JWTHandler.GenerarToken(usuario, configuracion);

            // No devolver la contraseña en la respuesta.
            usuario.Password = string.Empty;

            return Ok(new
            {
                Token = token,
                Usuario = usuario
            });
        }
        catch (UsuarioException uex)
        {
            return BadRequest(uex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
