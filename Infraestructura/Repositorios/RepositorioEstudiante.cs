using Dominio.Entidades;
using Dominio.Interfaces;
using Infraestructura.Datos;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositorios;

public class RepositorioEstudiante : IRepositorioEstudiante, IRepositorioExistenciaEstudiante
{
    private readonly ContextoBaseDatos contexto;

    public RepositorioEstudiante(ContextoBaseDatos contexto)
    {
        this.contexto = contexto;
    }

    public async Task<Estudiante?> ObtenerPorIdAsync(int id, CancellationToken cancelacionToken)
    {
        return await contexto.estudiantes.FirstOrDefaultAsync(e => e.id == id, cancelacionToken);
    }

    public async Task<List<Estudiante>> ObtenerPaginadoAsync(int numeroPagina, int tamanoPagina, CancellationToken cancelacionToken)
    {
        return await contexto.estudiantes
            .OrderBy(e => e.id)
            .Skip((numeroPagina - 1) * tamanoPagina)
            .Take(tamanoPagina)
            .ToListAsync(cancelacionToken);
    }

    public async Task<int> ContarAsync(CancellationToken cancelacionToken)
    {
        return await contexto.estudiantes.CountAsync(cancelacionToken);
    }

    public async Task<Estudiante> CrearAsync(Estudiante estudiante, CancellationToken cancelacionToken)
    {
        contexto.estudiantes.Add(estudiante);
        await contexto.SaveChangesAsync(cancelacionToken);
        return estudiante;
    }

    public async Task ActualizarAsync(Estudiante estudiante, CancellationToken cancelacionToken)
    {
        contexto.estudiantes.Update(estudiante);
        await contexto.SaveChangesAsync(cancelacionToken);
    }

    public async Task EliminarAsync(Estudiante estudiante, CancellationToken cancelacionToken)
    {
        contexto.estudiantes.Remove(estudiante);
        await contexto.SaveChangesAsync(cancelacionToken);
    }

    public async Task<bool> ExisteAsync(int id, CancellationToken cancelacionToken)
    {
        return await contexto.estudiantes.AnyAsync(e => e.id == id, cancelacionToken);
    }
}
