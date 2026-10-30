using Dominio.Enumerados;
using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Auditorias;
using LogicaAplicacion.InterfacesDeCasoDeUso.Historias;
using LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios;
using Microsoft.AspNetCore.Mvc;
using PresentacionAdmin.Filtros;

namespace PresentacionAdmin.Controllers;

// Sin alta manual ni Edit/Delete -- la auditoria la genera el sistema solo.
[RequiereSesion]
public class AuditoriaController : Controller
{
    private readonly IEncontrarTodasAuditorias encontrarTodasAuditorias;
    private readonly IObtenerAuditoriaPorId obtenerAuditoriaPorId;
    private readonly IEncontrarAuditoriasPorHistoria encontrarAuditoriasPorHistoria;
    private readonly IEncontrarAuditoriasPorAdministrador encontrarAuditoriasPorAdministrador;
    private readonly IEncontrarTodasHistorias encontrarTodasHistorias;
    private readonly IEncontrarTodosUsuarios encontrarTodosUsuarios;

    public AuditoriaController(
        IObtenerAuditoriaPorId obtenerAuditoriaPorIdCu,
        IEncontrarTodasAuditorias encontrarTodasAuditoriasCu,
        IEncontrarAuditoriasPorHistoria encontrarAuditoriasPorHistoriaCu,
        IEncontrarAuditoriasPorAdministrador encontrarAuditoriasPorAdministradorCu,
        IEncontrarTodasHistorias encontrarTodasHistoriasCu,
        IEncontrarTodosUsuarios encontrarTodosUsuariosCu)
    {
        obtenerAuditoriaPorId = obtenerAuditoriaPorIdCu;
        encontrarTodasAuditorias = encontrarTodasAuditoriasCu;
        encontrarAuditoriasPorHistoria = encontrarAuditoriasPorHistoriaCu;
        encontrarAuditoriasPorAdministrador = encontrarAuditoriasPorAdministradorCu;
        encontrarTodasHistorias = encontrarTodasHistoriasCu;
        encontrarTodosUsuarios = encontrarTodosUsuariosCu;
    }

    // Filtro opcional por historia o por administrador; sin ninguno, lista todas.
    public ActionResult Index(int? historiaId, int? administradorId)
    {
        List<AuditoriaDTO> auditorias;
        if (historiaId.HasValue)
            auditorias = encontrarAuditoriasPorHistoria.Ejecutar(historiaId.Value);
        else if (administradorId.HasValue)
            auditorias = encontrarAuditoriasPorAdministrador.Ejecutar(administradorId.Value);
        else
            auditorias = encontrarTodasAuditorias.Ejecutar();

        ViewBag.Historias = encontrarTodasHistorias.Ejecutar();
        ViewBag.Administradores = encontrarTodosUsuarios.Ejecutar()
            .Where(usuario => usuario.Rol == RolUsuario.Administrador)
            .ToList();
        ViewBag.HistoriaIdSeleccionada = historiaId;
        ViewBag.AdministradorIdSeleccionado = administradorId;

        return View(auditorias);
    }

    public ActionResult Details(int id)
    {
        // Mismas listas que Index, para mostrar nombres en vez de ids.
        ViewBag.Historias = encontrarTodasHistorias.Ejecutar();
        ViewBag.Administradores = encontrarTodosUsuarios.Ejecutar()
            .Where(usuario => usuario.Rol == RolUsuario.Administrador)
            .ToList();

        return View(obtenerAuditoriaPorId.Ejecutar(id));
    }
}
