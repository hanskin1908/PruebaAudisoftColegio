using Dominio.Interfaces;
using Infraestructura.Datos;
using Infraestructura.Repositorios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infraestructura.Configuracion;

public static class ConfiguracionInfraestructura
{
    public static IServiceCollection AgregarInfraestructura(this IServiceCollection servicios, IConfiguration configuracion)
    {
        var cadenaConexion = configuracion.GetConnectionString("BaseDatosColegio");
        servicios.AddDbContext<ContextoBaseDatos>(opciones => opciones.UseSqlServer(cadenaConexion));

        servicios.AddScoped<IRepositorioEstudiante, RepositorioEstudiante>();
        servicios.AddScoped<IRepositorioExistenciaEstudiante, RepositorioEstudiante>();
        servicios.AddScoped<IRepositorioProfesor, RepositorioProfesor>();
        servicios.AddScoped<IRepositorioExistenciaProfesor, RepositorioProfesor>();
        servicios.AddScoped<IRepositorioNota, RepositorioNota>();
        servicios.AddScoped<IRepositorioConsultaNotaEstudiante, RepositorioNota>();
        servicios.AddScoped<IRepositorioConsultaNotaProfesor, RepositorioNota>();

        return servicios;
    }
}
