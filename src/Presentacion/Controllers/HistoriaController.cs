using Dominio.Enumerados;
using Dominio.Excepciones;
using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Categorias;
using LogicaAplicacion.InterfacesDeCasoDeUso.Historias;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers;

public class HistoriaController : Controller
{
    private readonly IEncontrarTodasHistorias encontrarTodasHistorias;
    private readonly IAgregarHistoria agregarHistoria;
    private readonly IObtenerHistoriaPorId obtenerHistoriaPorId;
    private readonly IEditarHistoria editarHistoria;
    private readonly IEncontrarTodasCategorias encontrarTodasCategorias;

    public HistoriaController(
        IAgregarHistoria agregarHistoriaCu,
        IObtenerHistoriaPorId obtenerHistoriaPorIdCu,
        IEncontrarTodasHistorias encontrarTodasHistoriasCu,
        IEditarHistoria editarHistoriaCu,
        IEncontrarTodasCategorias encontrarTodasCategoriasCu)
    {
        agregarHistoria = agregarHistoriaCu;
        obtenerHistoriaPorId = obtenerHistoriaPorIdCu;
        encontrarTodasHistorias = encontrarTodasHistoriasCu;
        editarHistoria = editarHistoriaCu;
        encontrarTodasCategorias = encontrarTodasCategoriasCu;
    }

    public ActionResult Index()
    {
        return View(encontrarTodasHistorias.Ejecutar());
    }

    public ActionResult Details(int id)
    {
        return View(obtenerHistoriaPorId.Ejecutar(id));
    }

    public ActionResult Create()
    {
        ViewBag.Categorias = encontrarTodasCategorias.Ejecutar();
        return View();
    }

    // No recibe un HistoriaDTO completo porque HistoriaDTO.Capitulos es List<CapituloDTO>
    // (tipo abstracto) y el model binding de ASP.NET Core no puede instanciar eso desde
    // un formulario. Se arma el DTO a mano con los datos sueltos del form.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(string titulo, string sinopsis, List<int> categoriaIds,
        string capituloInicialTitulo, string capituloInicialTexto)
    {
        try
        {
            HistoriaDTO dto = new HistoriaDTO
            {
                Titulo = titulo,
                Sinopsis = sinopsis,
                Categorias = categoriaIds.Select(id => new CategoriaDTO { Id = id }).ToList(),
                Capitulos = new List<CapituloDTO>
                {
                    new CapituloIntermedioDTO
                    {
                        Titulo = capituloInicialTitulo,
                        Texto = capituloInicialTexto,
                        Opciones = new List<OpcionDTO>()
                    }
                }
            };

            agregarHistoria.Ejecutar(dto);
            return RedirectToAction(nameof(Index));
        }
        catch (DomainException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            ViewBag.Categorias = encontrarTodasCategorias.Ejecutar();
            return View();
        }
    }

    public ActionResult Edit(int id)
    {
        ViewBag.Categorias = encontrarTodasCategorias.Ejecutar();
        return View(obtenerHistoriaPorId.Ejecutar(id));
    }

    // Mismo motivo que en Create: se arman los campos sueltos en vez de bindear
    // HistoriaDTO completo (acá además no se tocan Capitulos, la edición de Historia
    // no los modifica).
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, string titulo, string sinopsis, List<int> categoriaIds, EstadoHistoria estado)
    {
        try
        {
            HistoriaDTO dto = new HistoriaDTO
            {
                Id = id,
                Titulo = titulo,
                Sinopsis = sinopsis,
                Categorias = categoriaIds.Select(categoriaId => new CategoriaDTO { Id = categoriaId }).ToList(),
                Estado = estado
            };

            editarHistoria.Ejecutar(dto);
            return RedirectToAction(nameof(Index));
        }
        catch (DomainException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            ViewBag.Categorias = encontrarTodasCategorias.Ejecutar();
            return View(obtenerHistoriaPorId.Ejecutar(id));
        }
    }
}
