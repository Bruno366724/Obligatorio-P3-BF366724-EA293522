using AccesoDatos.Repositorios;
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

// Add services to the container.
builder.Services.AddControllersWithViews();

//INICIALIZAMOS LOS REPOSITORIOS
builder.Services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
builder.Services.AddScoped<IRepositorioCategoria, RepositorioCategoria>();
builder.Services.AddScoped<IRepositorioHistoria, RepositorioHistoria>();
builder.Services.AddScoped<IRepositorioLectura, RepositorioLectura>();
builder.Services.AddScoped<IRepositorioAuditoria, RepositorioAuditoria>();

//INICIALIZAMOS LOS CASOS DE USO - USUARIOS
builder.Services.AddScoped<IAgregarUsuario, AgregarUsuarioCU>();
builder.Services.AddScoped<IObtenerUsuarioPorId, ObtenerUsuarioPorIdCU>();
builder.Services.AddScoped<IEncontrarTodosUsuarios, EncontrarTodosUsuariosCU>();

//INICIALIZAMOS LOS CASOS DE USO - CATEGORIAS
builder.Services.AddScoped<IAgregarCategoria, AgregarCategoriaCU>();
builder.Services.AddScoped<IObtenerCategoriaPorId, ObtenerCategoriaPorIdCU>();
builder.Services.AddScoped<IEncontrarTodasCategorias, EncontrarTodasCategoriasCU>();

//INICIALIZAMOS LOS CASOS DE USO - HISTORIAS
builder.Services.AddScoped<IAgregarHistoria, AgregarHistoriaCU>();
builder.Services.AddScoped<IObtenerHistoriaPorId, ObtenerHistoriaPorIdCU>();
builder.Services.AddScoped<IEncontrarTodasHistorias, EncontrarTodasHistoriasCU>();

//INICIALIZAMOS LOS CASOS DE USO - LECTURAS
builder.Services.AddScoped<IAgregarLectura, AgregarLecturaCU>();
builder.Services.AddScoped<IObtenerLecturaPorId, ObtenerLecturaPorIdCU>();
builder.Services.AddScoped<IEncontrarTodasLecturas, EncontrarTodasLecturasCU>();

//INICIALIZAMOS LOS CASOS DE USO - AUDITORIAS
builder.Services.AddScoped<IAgregarAuditoria, AgregarAuditoriaCU>();
builder.Services.AddScoped<IObtenerAuditoriaPorId, ObtenerAuditoriaPorIdCU>();
builder.Services.AddScoped<IEncontrarTodasAuditorias, EncontrarTodasAuditoriasCU>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
