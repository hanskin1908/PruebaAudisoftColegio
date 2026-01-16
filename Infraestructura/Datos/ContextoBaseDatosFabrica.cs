using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infraestructura.Datos;

public class ContextoBaseDatosFabrica : IDesignTimeDbContextFactory<ContextoBaseDatos>
{
    public ContextoBaseDatos CreateDbContext(string[] args)
    {
        var opciones = new DbContextOptionsBuilder<ContextoBaseDatos>();
        opciones.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=BaseDatosColegio;Trusted_Connection=True;MultipleActiveResultSets=true");
        return new ContextoBaseDatos(opciones.Options);
    }
}
