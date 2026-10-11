// Web API de Lectura (RF06+): login con JWT, historias, lecturas.

using System.Text;
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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using WebApiLectura.Servicios;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Esquema de seguridad para que Swagger muestre el boton "Authorize" con JWT.
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Pegar solo el token, sin el prefijo \"Bearer \" -- Swagger lo agrega solo."
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});

// Autenticacion por JWT. Clave/emisor/audiencia salen de appsettings.json ("Jwt").
IConfigurationSection configJwt = builder.Configuration.GetSection("Jwt");
string claveJwt = configJwt["Clave"]!;

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = configJwt["Emisor"],
            ValidateAudience = true,
            ValidAudience = configJwt["Audiencia"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(claveJwt)),
            ValidateLifetime = true
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddScoped<GeneradorTokenJwt>();

builder.Services.AddScoped<IRepositorioUsuario, RepositorioUsuarioEF>();
builder.Services.AddScoped<IRepositorioCategoria, RepositorioCategoriaEF>();
builder.Services.AddScoped<IRepositorioHistoria, RepositorioHistoriaEF>();
builder.Services.AddScoped<IRepositorioLectura, RepositorioLecturaEF>();
builder.Services.AddScoped<IRepositorioAuditoria, RepositorioAuditoriaEF>();

builder.Services.AddScoped<IAgregarUsuario, AgregarUsuarioCU>();
builder.Services.AddScoped<IObtenerUsuarioPorId, ObtenerUsuarioPorIdCU>();
builder.Services.AddScoped<IEncontrarTodosUsuarios, EncontrarTodosUsuariosCU>();
builder.Services.AddScoped<IIniciarSesion, IniciarSesionCU>();
builder.Services.AddScoped<IIniciarSesionLectura, IniciarSesionLecturaCU>();

builder.Services.AddScoped<IAgregarCategoria, AgregarCategoriaCU>();
builder.Services.AddScoped<IObtenerCategoriaPorId, ObtenerCategoriaPorIdCU>();
builder.Services.AddScoped<IEncontrarTodasCategorias, EncontrarTodasCategoriasCU>();

builder.Services.AddScoped<IAgregarHistoria, AgregarHistoriaCU>();
builder.Services.AddScoped<IObtenerHistoriaPorId, ObtenerHistoriaPorIdCU>();
builder.Services.AddScoped<IEncontrarTodasHistorias, EncontrarTodasHistoriasCU>();
builder.Services.AddScoped<IEditarHistoria, EditarHistoriaCU>();
builder.Services.AddScoped<IEncontrarHistoriasPublicadas, EncontrarHistoriasPublicadasCU>();

builder.Services.AddScoped<IAgregarLectura, AgregarLecturaCU>();
builder.Services.AddScoped<IObtenerLecturaPorId, ObtenerLecturaPorIdCU>();
builder.Services.AddScoped<IEncontrarTodasLecturas, EncontrarTodasLecturasCU>();
builder.Services.AddScoped<IIniciarLectura, IniciarLecturaCU>();
builder.Services.AddScoped<IAvanzarLectura, AvanzarLecturaCU>();
builder.Services.AddScoped<IEncontrarHistorialLecturas, EncontrarHistorialLecturasCU>();

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

// UseAuthentication siempre antes de UseAuthorization.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
