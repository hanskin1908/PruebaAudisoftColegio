namespace Dominio.Interfaces;

public interface IRepositorioConsultaNotaProfesor
{
    Task<bool> ExisteParaProfesorAsync(int idProfesor, CancellationToken cancelacionToken);
}
