using Dominio.Enumerados;
using Dominio.Excepciones;
using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Categorias;
using LogicaAplicacion.InterfacesDeCasoDeUso.Historias;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentacion.Filtros;

namespace Presentacion.Controllers;

[RequiereSesion]
public class HistoriaController : Controller
{
    private readonly IEncontrarTodasHistorias encontrarTodasHistorias;
    private readonly IAgregarHistoria agregarHistoria;
    private readonly IObtenerHistoriaPorId obtenerHistoriaPorId;
    private readonly IEditarHistoria editarHistoria;
    private readonly IEncontrarTodasCategorias encontrarTodasCategorias;
    private readonly IAgregarCapitulo agregarCapitulo;

    public HistoriaController(
        IAgregarHistoria agregarHistoriaCu,
        IObtenerHistoriaPorId obtenerHistoriaPorIdCu,
        IEncontrarTodasHistorias encontrarTodasHistoriasCu,
        IEditarHistoria editarHistoriaCu,
        IEncontrarTodasCategorias encontrarTodasCategoriasCu,
        IAgregarCapitulo agregarCapituloCu)
    {
        agregarHistoria = agregarHistoriaCu;
        obtenerHistoriaPorId = obtenerHistoriaPorIdCu;
        encontrarTodasHistorias = encontrarTodasHistoriasCu;
        editarHistoria = editarHistoriaCu;
        encontrarTodasCategorias = encontrarTodasCategoriasCu;
        agregarCapitulo = agregarCapituloCu;
    }

    // Usuario logueado, para los casos de uso que auditan.
    private int AdministradorId => HttpContext.Session.GetInt32("UsuarioId")!.Value;

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
        catch (DomainException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            ViewBag.Categorias = encontrarTodasCategorias.Ejecutar();
            return View(obtenerHistoriaPorId.Ejecutar(id));
        }
    }

    // Agrega un capitulo (intermedio o final) a continuacion de un capitulo intermedio.
    public ActionResult AgregarCapitulo(int historiaId, int capituloAntecedenteId)
    {
        HistoriaDTO historia = obtenerHistoriaPorId.Ejecutar(historiaId);
        ViewBag.HistoriaId = historiaId;
        ViewBag.HistoriaTitulo = historia.Titulo;
        ViewBag.CapituloAntecedenteId = capituloAntecedenteId;
        ViewBag.CapituloAntecedenteTitulo = historia.Capitulos
            .First(capitulo => capitulo.Id == capituloAntecedenteId).Titulo;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult AgregarCapitulo(int historiaId, int capituloAntecedenteId, bool esFinal,
        string titulo, string texto, string textoOpcion)
    {
        try
        {
            agregarCapitulo.Ejecutar(historiaId, capituloAntecedenteId, esFinal, titulo, texto, textoOpcion, AdministradorId);
            return RedirectToAction(nameof(Details), new { id = historiaId });
        }
        catch (DomainException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            HistoriaDTO historia = obtenerHistoriaPorId.Ejecutar(historiaId);
            ViewBag.HistoriaId = historiaId;
            ViewBag.HistoriaTitulo = historia.Titulo;
            ViewBag.CapituloAntecedenteId = capituloAntecedenteId;
            ViewBag.CapituloAntecedenteTitulo = historia.Capitulos
                .First(capitulo => capitulo.Id == capituloAntecedenteId).Titulo;
            return View();
        }
    }
}
