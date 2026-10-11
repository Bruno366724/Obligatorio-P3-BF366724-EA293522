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

    // GET: api/Usuario
    [HttpGet]
    public ActionResult<IEnumerable<object>> Get()
    {
        return Ok(encontrarTodosUsuarios.Ejecutar().Select(SinPassword));
    }

    // GET: api/Usuario/5
    [HttpGet("{id}")]
    public ActionResult<object> Get(int id)
    {
        try
        {
            return Ok(SinPassword(obtenerUsuarioPorId.Ejecutar(id)));
        }
        catch (DomainException ex)
        {
            return NotFound(ex.Message);
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
