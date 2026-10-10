using Dominio.Excepciones;
using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLectura.Controllers;

// Solo lectura por ahora, igual que lo que el profe mostro en ApiWeb/UsuarioController
// de RedSocialM3C (ahi tambien solo el Get() esta wireado a un caso de uso real).
// No expone Password en la respuesta -- UsuarioDTO lo trae en texto plano (se usa para
// el alta en RF02), pero no tiene sentido devolverlo en un endpoint de lectura.
[Route("api/[controller]")]
[ApiController]
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
