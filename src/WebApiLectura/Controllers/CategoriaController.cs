using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Categorias;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLectura.Controllers;

// Solo lectura -- para que el lado de Lectura pueda filtrar historias por categoria.
[Route("api/[controller]")]
[ApiController]
public class CategoriaController : ControllerBase
{
    private readonly IEncontrarTodasCategorias encontrarTodasCategorias;

    public CategoriaController(IEncontrarTodasCategorias encontrarTodasCategoriasCu)
    {
        encontrarTodasCategorias = encontrarTodasCategoriasCu;
    }

    // GET: api/Categoria
    [HttpGet]
    public ActionResult<List<CategoriaDTO>> Get()
    {
        return Ok(encontrarTodasCategorias.Ejecutar());
    }
}
