using Dominio.Entidades;

namespace Dominio.Interfaces;

public interface IRepositorioEstudiante
{
    Task<Estudiante?> ObtenerPorIdAsync(int id, CancellationToken cancelacionToken);
    Task<List<Estudiante>> ObtenerPaginadoAsync(int numeroPagina, int tamanoPagina, CancellationToken cancelacionToken);
    Task<int> ContarAsync(CancellationToken cancelacionToken);
    Task<Estudiante> CrearAsync(Estudiante estudiante, CancellationToken cancelacionToken);
    Task ActualizarAsync(Estudiante estudiante, CancellationToken cancelacionToken);
    Task EliminarAsync(Estudiante estudiante, CancellationToken cancelacionToken);
}
