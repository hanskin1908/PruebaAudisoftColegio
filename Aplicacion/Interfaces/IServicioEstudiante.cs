using Aplicacion.Modelos.Paginacion;
using Aplicacion.Modelos.Respuestas;
using Aplicacion.Modelos.Solicitudes;

namespace Aplicacion.Interfaces;

public interface IServicioEstudiante
{
    Task<ListaPaginadaRespuesta<EstudianteRespuesta>> ObtenerPaginadoAsync(int numeroPagina, int tamanoPagina, CancellationToken cancelacionToken);
    Task<EstudianteRespuesta?> ObtenerPorIdAsync(int id, CancellationToken cancelacionToken);
    Task<EstudianteRespuesta> CrearAsync(EstudianteCrearSolicitud solicitud, CancellationToken cancelacionToken);
    Task<bool> ActualizarAsync(int id, EstudianteActualizarSolicitud solicitud, CancellationToken cancelacionToken);
    Task<bool> EliminarAsync(int id, CancellationToken cancelacionToken);
}
