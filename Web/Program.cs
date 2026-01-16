using Microsoft.Extensions.Options;
using Presentacion.Modelos;
using Presentacion.Servicios;

var constructor = WebApplication.CreateBuilder(args);

constructor.Services.AddControllersWithViews();
constructor.Services.Configure<ConfiguracionApi>(constructor.Configuration.GetSection("ConfiguracionApi"));
constructor.Services.AddHttpClient<IServicioApiEstudiante, ServicioApiEstudiante>((proveedor, cliente) =>
{
    var opciones = proveedor.GetRequiredService<IOptions<ConfiguracionApi>>().Value;
    cliente.BaseAddress = new Uri(opciones.direccionApiBase);
});
constructor.Services.AddHttpClient<IServicioApiConsultaEstudiante, ServicioApiEstudiante>((proveedor, cliente) =>
{
    var opciones = proveedor.GetRequiredService<IOptions<ConfiguracionApi>>().Value;
    cliente.BaseAddress = new Uri(opciones.direccionApiBase);
});
constructor.Services.AddHttpClient<IServicioApiProfesor, ServicioApiProfesor>((proveedor, cliente) =>
{
    var opciones = proveedor.GetRequiredService<IOptions<ConfiguracionApi>>().Value;
    cliente.BaseAddress = new Uri(opciones.direccionApiBase);
});
constructor.Services.AddHttpClient<IServicioApiConsultaProfesor, ServicioApiProfesor>((proveedor, cliente) =>
{
    var opciones = proveedor.GetRequiredService<IOptions<ConfiguracionApi>>().Value;
    cliente.BaseAddress = new Uri(opciones.direccionApiBase);
});
constructor.Services.AddHttpClient<IServicioApiNota, ServicioApiNota>((proveedor, cliente) =>
{
    var opciones = proveedor.GetRequiredService<IOptions<ConfiguracionApi>>().Value;
    cliente.BaseAddress = new Uri(opciones.direccionApiBase);
});

var aplicacion = constructor.Build();

// Configura el pipeline HTTP.
if (!aplicacion.Environment.IsDevelopment())
{
    aplicacion.UseExceptionHandler("/Inicio/Error");
    aplicacion.UseHsts();
}

aplicacion.UseHttpsRedirection();
aplicacion.UseStaticFiles();

aplicacion.UseRouting();

aplicacion.UseAuthorization();

aplicacion.MapControllerRoute(
    name: "predeterminado",
    pattern: "{controller=Inicio}/{action=Inicio}/{id?}");

aplicacion.Run();
