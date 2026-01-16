using Presentacion.Modelos.Paginacion;
using Presentacion.Modelos.Respuestas;
using Presentacion.Modelos.Solicitudes;

namespace Presentacion.Servicios;

public interface IServicioApiProfesor
{
    Task<ListaPaginadaRespuesta<ProfesorRespuesta>> ObtenerPaginadoAsync(int numeroPagina, int tamanoPagina, CancellationToken cancelacionToken);
    Task<ProfesorRespuesta?> ObtenerPorIdAsync(int id, CancellationToken cancelacionToken);
    Task<string?> CrearAsync(ProfesorCrearSolicitud solicitud, CancellationToken cancelacionToken);
    Task<string?> ActualizarAsync(int id, ProfesorActualizarSolicitud solicitud, CancellationToken cancelacionToken);
    Task<string?> EliminarAsync(int id, CancellationToken cancelacionToken);
}
