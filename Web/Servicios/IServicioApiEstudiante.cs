using Presentacion.Modelos.Paginacion;
using Presentacion.Modelos.Respuestas;
using Presentacion.Modelos.Solicitudes;

namespace Presentacion.Servicios;

public interface IServicioApiEstudiante
{
    Task<ListaPaginadaRespuesta<EstudianteRespuesta>> ObtenerPaginadoAsync(int numeroPagina, int tamanoPagina, CancellationToken cancelacionToken);
    Task<EstudianteRespuesta?> ObtenerPorIdAsync(int id, CancellationToken cancelacionToken);
    Task<string?> CrearAsync(EstudianteCrearSolicitud solicitud, CancellationToken cancelacionToken);
    Task<string?> ActualizarAsync(int id, EstudianteActualizarSolicitud solicitud, CancellationToken cancelacionToken);
    Task<string?> EliminarAsync(int id, CancellationToken cancelacionToken);
}
