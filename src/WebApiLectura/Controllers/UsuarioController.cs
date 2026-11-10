using System.Net;
using Dominio.Excepciones;
using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLectura.Controllers;

// Listar/ver usuarios: restringido a Administrador.
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Administrador")]
public class UsuarioController : ControllerBase
{
    private readonly IEncontrarTodosUsuarios encontrarTodosUsuarios;
    private readonly IObtenerUsuarioPorId obtenerUsuarioPorId;

    public UsuarioController(
        IEncontrarTodosUsuarios encontrarTodosUsuariosCu,
        IObtenerUsuarioPorId obtenerUsuarioPorIdCu)
    {
        encontrarTodosUsuarios = encontrarTodosUsuariosCu;
        obtenerUsuarioPorId = obtenerUsuarioPorIdCu;
    }

    /// <summary>Lista todos los usuarios.</summary>
    /// <response code="200">Listado de usuarios.</response>
    /// <response code="403">El usuario no es Administrador.</response>
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public ActionResult<IEnumerable<object>> Get()
    {
        try
        {
            return Ok(encontrarTodosUsuarios.Ejecutar().Select(SinPassword));
        }
        catch (Exception)
        {
            return StatusCode(500, "Error inesperado, vuelva a intentar más tarde.");
        }
    }

    /// <summary>Devuelve un usuario por id.</summary>
    /// <param name="id">Id del usuario.</param>
    /// <response code="200">El usuario pedido.</response>
    /// <response code="403">El usuario no es Administrador.</response>
    /// <response code="404">El usuario no existe.</response>
    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public ActionResult<object> Get(int id)
    {
        try
        {
            return Ok(SinPassword(obtenerUsuarioPorId.Ejecutar(id)));
        }
        catch (UsuarioException uex)
        {
            return NotFound(uex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Error inesperado, vuelva a intentar más tarde.");
        }
    }

    private static object SinPassword(UsuarioDTO usuario)
    {
        return new
        {
            usuario.Id,
            usuario.NombreCompleto,
            usuario.Email,
            usuario.NombreUsuario,
            usuario.Rol
        };
    }
}
