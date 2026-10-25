// App MVC cliente de Lectura: consume la Web API por HttpClient.

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Session: guarda el token JWT y los datos del usuario logueado.
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

// HttpClient con nombre, URL base en appsettings.json ("WebApiLectura:BaseUrl").
builder.Services.AddHttpClient("WebApiLectura", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["WebApiLectura:BaseUrl"]!);
});

WebApplication app = builder.Build();

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
    pattern: "{controller=Auth}/{action=Login}/{id?}")
    .WithStaticAssets();

app.Run();
