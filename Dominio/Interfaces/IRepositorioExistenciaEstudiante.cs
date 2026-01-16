namespace Dominio.Interfaces;

public interface IRepositorioExistenciaEstudiante
{
    Task<bool> ExisteAsync(int id, CancellationToken cancelacionToken);
}
