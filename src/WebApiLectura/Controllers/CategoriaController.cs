using System.Net;
using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Categorias;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLectura.Controllers;

// Solo lectura, requiere JWT valido (cualquier rol).
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoriaController : ControllerBase
{
    private readonly IEncontrarTodasCategorias encontrarTodasCategorias;

    public CategoriaController(IEncontrarTodasCategorias encontrarTodasCategoriasCu)
    {
        encontrarTodasCategorias = encontrarTodasCategoriasCu;
    }

    // GET: api/Categoria
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public ActionResult<List<CategoriaDTO>> Get()
    {
        try
        {
            return Ok(encontrarTodasCategorias.Ejecutar());
        }
        catch (Exception)
        {
            return StatusCode(500, "Error inesperado, vuelva a intentar más tarde.");
        }
    }
}
