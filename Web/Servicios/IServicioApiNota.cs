using Presentacion.Modelos.Paginacion;
using Presentacion.Modelos.Respuestas;
using Presentacion.Modelos.Solicitudes;

namespace Presentacion.Servicios;

public interface IServicioApiNota
{
    Task<ListaPaginadaRespuesta<NotaRespuesta>> ObtenerPaginadoAsync(int numeroPagina, int tamanoPagina, CancellationToken cancelacionToken);
    Task<NotaRespuesta?> ObtenerPorIdAsync(int id, CancellationToken cancelacionToken);
    Task<string?> CrearAsync(NotaCrearSolicitud solicitud, CancellationToken cancelacionToken);
    Task<string?> ActualizarAsync(int id, NotaActualizarSolicitud solicitud, CancellationToken cancelacionToken);
    Task<string?> EliminarAsync(int id, CancellationToken cancelacionToken);
}
