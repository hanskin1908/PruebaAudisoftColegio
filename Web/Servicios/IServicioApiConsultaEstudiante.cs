using Presentacion.Modelos.Paginacion;
using Presentacion.Modelos.Respuestas;

namespace Presentacion.Servicios;

public interface IServicioApiConsultaEstudiante
{
    Task<ListaPaginadaRespuesta<EstudianteRespuesta>> ObtenerPaginadoAsync(int numeroPagina, int tamanoPagina, CancellationToken cancelacionToken);
    Task<EstudianteRespuesta?> ObtenerPorIdAsync(int id, CancellationToken cancelacionToken);
}
