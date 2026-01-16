using Dominio.Entidades;

namespace Dominio.Interfaces;

public interface IRepositorioProfesor
{
    Task<Profesor?> ObtenerPorIdAsync(int id, CancellationToken cancelacionToken);
    Task<List<Profesor>> ObtenerPaginadoAsync(int numeroPagina, int tamanoPagina, CancellationToken cancelacionToken);
    Task<int> ContarAsync(CancellationToken cancelacionToken);
    Task<Profesor> CrearAsync(Profesor profesor, CancellationToken cancelacionToken);
    Task ActualizarAsync(Profesor profesor, CancellationToken cancelacionToken);
    Task EliminarAsync(Profesor profesor, CancellationToken cancelacionToken);
}
