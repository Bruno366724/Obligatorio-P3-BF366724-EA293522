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

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

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

builder.Services.AddScoped<IAgregarLectura, AgregarLecturaCU>();
builder.Services.AddScoped<IObtenerLecturaPorId, ObtenerLecturaPorIdCU>();
builder.Services.AddScoped<IEncontrarTodasLecturas, EncontrarTodasLecturasCU>();

builder.Services.AddScoped<IAgregarAuditoria, AgregarAuditoriaCU>();
builder.Services.AddScoped<IObtenerAuditoriaPorId, ObtenerAuditoriaPorIdCU>();
builder.Services.AddScoped<IEncontrarTodasAuditorias, EncontrarTodasAuditoriasCU>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Usuario}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
