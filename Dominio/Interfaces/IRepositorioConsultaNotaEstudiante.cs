namespace Dominio.Interfaces;

public interface IRepositorioConsultaNotaEstudiante
{
    Task<bool> ExisteParaEstudianteAsync(int idEstudiante, CancellationToken cancelacionToken);
}
