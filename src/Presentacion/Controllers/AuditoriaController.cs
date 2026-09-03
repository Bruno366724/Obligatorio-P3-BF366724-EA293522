using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Auditorias;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers;

public class AuditoriaController : Controller
{
    private readonly IEncontrarTodasAuditorias encontrarTodasAuditorias;
    private readonly IObtenerAuditoriaPorId obtenerAuditoriaPorId;

    public AuditoriaController(
        IObtenerAuditoriaPorId obtenerAuditoriaPorIdCu,
        IEncontrarTodasAuditorias encontrarTodasAuditoriasCu)
    {
        obtenerAuditoriaPorId = obtenerAuditoriaPorIdCu;
        encontrarTodasAuditorias = encontrarTodasAuditoriasCu;
    }

    // GET: AuditoriaController
    public ActionResult Index()
    {
        return View(encontrarTodasAuditorias.Ejecutar());
    }

    // GET: AuditoriaController/Details/5
    public ActionResult Details(int id)
    {
        return View(obtenerAuditoriaPorId.Ejecutar(id));
    }
}
