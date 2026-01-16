using Dominio.Entidades;

namespace Dominio.Interfaces;

public interface IRepositorioNota
{
    Task<Nota?> ObtenerPorIdAsync(int id, CancellationToken cancelacionToken);
    Task<List<Nota>> ObtenerPaginadoAsync(int numeroPagina, int tamanoPagina, CancellationToken cancelacionToken);
    Task<int> ContarAsync(CancellationToken cancelacionToken);
    Task<Nota> CrearAsync(Nota nota, CancellationToken cancelacionToken);
    Task ActualizarAsync(Nota nota, CancellationToken cancelacionToken);
    Task EliminarAsync(Nota nota, CancellationToken cancelacionToken);
}
