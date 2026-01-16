using Dominio.Entidades;
using Dominio.Interfaces;
using Infraestructura.Datos;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositorios;

public class RepositorioNota : IRepositorioNota, IRepositorioConsultaNotaEstudiante, IRepositorioConsultaNotaProfesor
{
    private readonly ContextoBaseDatos contexto;

    public RepositorioNota(ContextoBaseDatos contexto)
    {
        this.contexto = contexto;
    }

    public async Task<Nota?> ObtenerPorIdAsync(int id, CancellationToken cancelacionToken)
    {
        return await contexto.notas.FirstOrDefaultAsync(n => n.id == id, cancelacionToken);
    }

    public async Task<List<Nota>> ObtenerPaginadoAsync(int numeroPagina, int tamanoPagina, CancellationToken cancelacionToken)
    {
        return await contexto.notas
            .OrderBy(n => n.id)
            .Skip((numeroPagina - 1) * tamanoPagina)
            .Take(tamanoPagina)
            .ToListAsync(cancelacionToken);
    }

    public async Task<int> ContarAsync(CancellationToken cancelacionToken)
    {
        return await contexto.notas.CountAsync(cancelacionToken);
    }

    public async Task<Nota> CrearAsync(Nota nota, CancellationToken cancelacionToken)
    {
        contexto.notas.Add(nota);
        await contexto.SaveChangesAsync(cancelacionToken);
        return nota;
    }

    public async Task ActualizarAsync(Nota nota, CancellationToken cancelacionToken)
    {
        contexto.notas.Update(nota);
        await contexto.SaveChangesAsync(cancelacionToken);
    }

    public async Task EliminarAsync(Nota nota, CancellationToken cancelacionToken)
    {
        contexto.notas.Remove(nota);
        await contexto.SaveChangesAsync(cancelacionToken);
    }

    public async Task<bool> ExisteParaEstudianteAsync(int idEstudiante, CancellationToken cancelacionToken)
    {
        return await contexto.notas.AnyAsync(n => n.idEstudiante == idEstudiante, cancelacionToken);
    }

    public async Task<bool> ExisteParaProfesorAsync(int idProfesor, CancellationToken cancelacionToken)
    {
        return await contexto.notas.AnyAsync(n => n.idProfesor == idProfesor, cancelacionToken);
    }
}
