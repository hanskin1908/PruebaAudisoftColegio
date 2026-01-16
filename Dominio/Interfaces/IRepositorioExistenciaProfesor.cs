namespace Dominio.Interfaces;

public interface IRepositorioExistenciaProfesor
{
    Task<bool> ExisteAsync(int id, CancellationToken cancelacionToken);
}
