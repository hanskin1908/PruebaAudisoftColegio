using Aplicacion.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Aplicacion.Servicios;

public static class ConfiguracionAplicacion
{
    public static IServiceCollection AgregarAplicacion(this IServiceCollection servicios)
    {
        servicios.AddScoped<IServicioEstudiante, ServicioEstudiante>();
        servicios.AddScoped<IServicioProfesor, ServicioProfesor>();
        servicios.AddScoped<IServicioNota, ServicioNota>();
        return servicios;
    }
}
