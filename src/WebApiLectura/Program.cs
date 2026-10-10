// Esqueleto de la Web API de Lectura (RF06+). Todavia no tiene JWT ni [Authorize] en
// ningun lado -- el profe recien empezo a mostrar Web API en RedSocialM3C (commit
// "Jugamos con webapi", 5/10) y ahi tampoco hay JWT todavia. La idea es tener ya armada
// la base (DI, Swagger, controllers de solo lectura) para poder enchufar la autenticacion
// encima apenas se de el tema en clase, en vez de arrancar de cero.
//
// Por ahora vive en la misma solucion (Obligatorio.slnx) junto con Presentacion (la app
// de Administracion). El enunciado pide dos soluciones .NET separadas para el entregable
// final -- separar esto en su propia solucion queda pendiente, no es parte de este paso.

using AccesoDatos.RepositorioEntityFramework.Repositorios;
using Dominio.InterfacesRepositorios;

using LogicaAplicacion.CasosDeUso.Auditorias;
using LogicaAplicacion.CasosDeUso.Categorias;
using LogicaAplicacion.CasosDeUso.Historias;
using LogicaAplicacion.CasosDeUso.Lecturas;
using LogicaAplicacion.CasosDeUso.Usuarios;
using LogicaAplicacion.InterfacesDeCasoDeUso.Auditorias;
using LogicaAplicacion.InterfacesDeCasoDeUso.Categorias;
using LogicaAplicacion.InterfacesDeCasoDeUso.Historias;
using LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas;
using LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

// Mismos repositorios y casos de uso que Presentacion (Program.cs) -- es el mismo
// agregado de Dominio, solo cambia quien lo consume. Lectura y Auditoria se registran
// igual aunque sus casos de uso todavia sean en su mayoria stubs NotImplementedException
// (ver CLAUDE.md); no hay controller que los use por ahora, asi que no se llegan a invocar.

builder.Services.AddScoped<IRepositorioUsuario, RepositorioUsuarioEF>();
builder.Services.AddScoped<IRepositorioCategoria, RepositorioCategoriaEF>();
builder.Services.AddScoped<IRepositorioHistoria, RepositorioHistoriaEF>();
builder.Services.AddScoped<IRepositorioLectura, RepositorioLecturaEF>();
builder.Services.AddScoped<IRepositorioAuditoria, RepositorioAuditoriaEF>();

builder.Services.AddScoped<IAgregarUsuario, AgregarUsuarioCU>();
builder.Services.AddScoped<IObtenerUsuarioPorId, ObtenerUsuarioPorIdCU>();
builder.Services.AddScoped<IEncontrarTodosUsuarios, EncontrarTodosUsuariosCU>();
builder.Services.AddScoped<IIniciarSesion, IniciarSesionCU>();

builder.Services.AddScoped<IAgregarCategoria, AgregarCategoriaCU>();
builder.Services.AddScoped<IObtenerCategoriaPorId, ObtenerCategoriaPorIdCU>();
builder.Services.AddScoped<IEncontrarTodasCategorias, EncontrarTodasCategoriasCU>();

builder.Services.AddScoped<IAgregarHistoria, AgregarHistoriaCU>();
builder.Services.AddScoped<IObtenerHistoriaPorId, ObtenerHistoriaPorIdCU>();
builder.Services.AddScoped<IEncontrarTodasHistorias, EncontrarTodasHistoriasCU>();
builder.Services.AddScoped<IEditarHistoria, EditarHistoriaCU>();

builder.Services.AddScoped<IAgregarLectura, AgregarLecturaCU>();
builder.Services.AddScoped<IObtenerLecturaPorId, ObtenerLecturaPorIdCU>();
builder.Services.AddScoped<IEncontrarTodasLecturas, EncontrarTodasLecturasCU>();

builder.Services.AddScoped<IAgregarAuditoria, AgregarAuditoriaCU>();
builder.Services.AddScoped<IObtenerAuditoriaPorId, ObtenerAuditoriaPorIdCU>();
builder.Services.AddScoped<IEncontrarTodasAuditorias, EncontrarTodasAuditoriasCU>();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Todavia no hay app.UseAuthentication() -- no hay JWT configurado. Cuando se agregue,
// va antes de UseAuthorization().
app.UseAuthorization();

app.MapControllers();

app.Run();
