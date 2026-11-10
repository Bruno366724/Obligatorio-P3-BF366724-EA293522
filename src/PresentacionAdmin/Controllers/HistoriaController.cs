using Dominio.Enumerados;
using Dominio.Excepciones;
using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Categorias;
using LogicaAplicacion.InterfacesDeCasoDeUso.Historias;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PresentacionAdmin.Filtros;

namespace PresentacionAdmin.Controllers;

[RequiereSesion]
public class HistoriaController : Controller
{
    private readonly IEncontrarTodasHistorias encontrarTodasHistorias;
    private readonly IAgregarHistoria agregarHistoria;
    private readonly IObtenerHistoriaPorId obtenerHistoriaPorId;
    private readonly IEditarHistoria editarHistoria;
    private readonly IEncontrarTodasCategorias encontrarTodasCategorias;
    private readonly IAgregarCapitulo agregarCapitulo;
    private readonly IEliminarHistoria eliminarHistoria;
    private readonly IAgregarOpcion agregarOpcion;

    public HistoriaController(
        IAgregarHistoria agregarHistoriaCu,
        IObtenerHistoriaPorId obtenerHistoriaPorIdCu,
        IEncontrarTodasHistorias encontrarTodasHistoriasCu,
        IEditarHistoria editarHistoriaCu,
        IEncontrarTodasCategorias encontrarTodasCategoriasCu,
        IAgregarCapitulo agregarCapituloCu,
        IEliminarHistoria eliminarHistoriaCu,
        IAgregarOpcion agregarOpcionCu)
    {
        agregarHistoria = agregarHistoriaCu;
        obtenerHistoriaPorId = obtenerHistoriaPorIdCu;
        encontrarTodasHistorias = encontrarTodasHistoriasCu;
        editarHistoria = editarHistoriaCu;
        encontrarTodasCategorias = encontrarTodasCategoriasCu;
        agregarCapitulo = agregarCapituloCu;
        eliminarHistoria = eliminarHistoriaCu;
        agregarOpcion = agregarOpcionCu;
    }

    // Usuario logueado, para los casos de uso que auditan.
    private int AdministradorId => HttpContext.Session.GetInt32("UsuarioId")!.Value;

    public ActionResult Index()
    {
        return View(encontrarTodasHistorias.Ejecutar());
    }

    public ActionResult Details(int id)
    {
        try
        {
            return View(obtenerHistoriaPorId.Ejecutar(id));
        }
        catch (HistoriaException hex)
        {
            TempData["Error"] = hex.Message;
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            TempData["Error"] = "Error inesperado al cargar la historia.";
            return RedirectToAction(nameof(Index));
        }
    }

    public ActionResult Create()
    {
        ViewBag.Categorias = encontrarTodasCategorias.Ejecutar();
        return View();
    }

    // Recibe campos sueltos en vez de un HistoriaDTO completo (Capitulos es abstracto).
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

            agregarHistoria.Ejecutar(dto, AdministradorId);
            return RedirectToAction(nameof(Index));
        }
        catch (HistoriaException hex)
        {
            ModelState.AddModelError(string.Empty, hex.Message);
            ViewBag.Categorias = encontrarTodasCategorias.Ejecutar();
            return View();
        }
        catch
        {
            ModelState.AddModelError(string.Empty, "Error inesperado al crear la historia, vuelva a intentar.");
            ViewBag.Categorias = encontrarTodasCategorias.Ejecutar();
            return View();
        }
    }

    public ActionResult Edit(int id)
    {
        try
        {
            ViewBag.Categorias = encontrarTodasCategorias.Ejecutar();
            return View(obtenerHistoriaPorId.Ejecutar(id));
        }
        catch (HistoriaException hex)
        {
            TempData["Error"] = hex.Message;
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            TempData["Error"] = "Error inesperado al cargar la historia.";
            return RedirectToAction(nameof(Index));
        }
    }

    // Campos sueltos, igual que en Create. La edición no toca Capitulos.
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

            editarHistoria.Ejecutar(dto, AdministradorId);
            return RedirectToAction(nameof(Index));
        }
        catch (HistoriaException hex)
        {
            ModelState.AddModelError(string.Empty, hex.Message);
            return Edit(id);
        }
        catch
        {
            ModelState.AddModelError(string.Empty, "Error inesperado al editar la historia, vuelva a intentar.");
            return Edit(id);
        }
    }

    // Pantalla de confirmacion de la baja.
    public ActionResult Delete(int id)
    {
        try
        {
            return View(obtenerHistoriaPorId.Ejecutar(id));
        }
        catch (HistoriaException hex)
        {
            TempData["Error"] = hex.Message;
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            TempData["Error"] = "Error inesperado al cargar la historia.";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmado(int id)
    {
        try
        {
            eliminarHistoria.Ejecutar(id, AdministradorId);
            TempData["Mensaje"] = "Historia eliminada correctamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (HistoriaException hex)
        {
            ModelState.AddModelError(string.Empty, hex.Message);
            return Delete(id);
        }
        catch
        {
            ModelState.AddModelError(string.Empty, "Error inesperado al eliminar la historia, vuelva a intentar.");
            return Delete(id);
        }
    }

    // Agrega un capitulo (intermedio o final) a continuacion de un capitulo intermedio.

    public ActionResult AgregarCapitulo(int historiaId, int capituloAntecedenteId)
    {
        try
        {
            HistoriaDTO historia = obtenerHistoriaPorId.Ejecutar(historiaId);
            ViewBag.HistoriaId = historiaId;
            ViewBag.HistoriaTitulo = historia.Titulo;
            ViewBag.CapituloAntecedenteId = capituloAntecedenteId;
            ViewBag.CapituloAntecedenteTitulo = historia.Capitulos
                .First(capitulo => capitulo.Id == capituloAntecedenteId).Titulo;
            return View();
        }
        catch (HistoriaException hex)
        {
            TempData["Error"] = hex.Message;
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            TempData["Error"] = "No se encontró el capítulo indicado en esta historia.";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult AgregarCapitulo(AgregarCapituloDTO dto)
    {
        try
        {
            agregarCapitulo.Ejecutar(dto, AdministradorId);
            return RedirectToAction(nameof(Details), new { id = dto.HistoriaId });
        }
        catch (HistoriaException hex)
        {
            ModelState.AddModelError(string.Empty, hex.Message);
            return AgregarCapitulo(dto.HistoriaId, dto.CapituloAntecedenteId);
        }
        catch
        {
            ModelState.AddModelError(string.Empty, "Error inesperado al agregar el capítulo, vuelva a intentar.");
            return AgregarCapitulo(dto.HistoriaId, dto.CapituloAntecedenteId);
        }
    }
    // Enlaza un capitulo intermedio con otro capitulo ya existente de la misma historia.
    public ActionResult AgregarOpcion(int historiaId, int capituloOrigenId)
    {
        try
        {
            HistoriaDTO historia = obtenerHistoriaPorId.Ejecutar(historiaId);
            CapituloIntermedioDTO origen = historia.Capitulos.OfType<CapituloIntermedioDTO>()
                .First(capitulo => capitulo.Id == capituloOrigenId);

            ViewBag.HistoriaId = historiaId;
            ViewBag.HistoriaTitulo = historia.Titulo;
            ViewBag.CapituloOrigenId = capituloOrigenId;
            ViewBag.CapituloOrigenTitulo = origen.Titulo;
            // Candidatos: todos menos el origen, el inicial y los que ya son destino del origen.
            ViewBag.Destinos = historia.Capitulos
                .Where(capitulo => capitulo.Id != capituloOrigenId
                    && capitulo.Id != historia.CapituloInicialId
                    && origen.Opciones.All(opcion => opcion.CapituloDestinoId != capitulo.Id))
                .ToList();
            return View();
        }
        catch (HistoriaException hex)
        {
            TempData["Error"] = hex.Message;
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            TempData["Error"] = "No se encontró el capítulo intermedio indicado en esta historia.";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult AgregarOpcion(AgregarOpcionDTO dto)
    {
        try
        {
            agregarOpcion.Ejecutar(dto, AdministradorId);
            return RedirectToAction(nameof(Details), new { id = dto.HistoriaId });
        }
        catch (HistoriaException hex)
        {
            ModelState.AddModelError(string.Empty, hex.Message);
            return AgregarOpcion(dto.HistoriaId, dto.CapituloOrigenId);
        }
        catch
        {
            ModelState.AddModelError(string.Empty, "Error inesperado al agregar la opción, vuelva a intentar.");
            return AgregarOpcion(dto.HistoriaId, dto.CapituloOrigenId);
        }
    }
}
