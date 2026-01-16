using Dominio.Entidades;
using Dominio.Interfaces;
using Infraestructura.Datos;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositorios;

public class RepositorioProfesor : IRepositorioProfesor, IRepositorioExistenciaProfesor
{
    private readonly ContextoBaseDatos contexto;

    public RepositorioProfesor(ContextoBaseDatos contexto)
    {
        this.contexto = contexto;
    }

    public async Task<Profesor?> ObtenerPorIdAsync(int id, CancellationToken cancelacionToken)
    {
        return await contexto.profesores.FirstOrDefaultAsync(p => p.id == id, cancelacionToken);
    }

    public async Task<List<Profesor>> ObtenerPaginadoAsync(int numeroPagina, int tamanoPagina, CancellationToken cancelacionToken)
    {
        return await contexto.profesores
            .OrderBy(p => p.id)
            .Skip((numeroPagina - 1) * tamanoPagina)
            .Take(tamanoPagina)
            .ToListAsync(cancelacionToken);
    }

    public async Task<int> ContarAsync(CancellationToken cancelacionToken)
    {
        return await contexto.profesores.CountAsync(cancelacionToken);
    }

    public async Task<Profesor> CrearAsync(Profesor profesor, CancellationToken cancelacionToken)
    {
        contexto.profesores.Add(profesor);
        await contexto.SaveChangesAsync(cancelacionToken);
        return profesor;
    }

    public async Task ActualizarAsync(Profesor profesor, CancellationToken cancelacionToken)
    {
        contexto.profesores.Update(profesor);
        await contexto.SaveChangesAsync(cancelacionToken);
    }

    public async Task EliminarAsync(Profesor profesor, CancellationToken cancelacionToken)
    {
        contexto.profesores.Remove(profesor);
        await contexto.SaveChangesAsync(cancelacionToken);
    }

    public async Task<bool> ExisteAsync(int id, CancellationToken cancelacionToken)
    {
        return await contexto.profesores.AnyAsync(p => p.id == id, cancelacionToken);
    }
}
