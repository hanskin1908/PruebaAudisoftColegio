using Aplicacion.Modelos.Paginacion;
using Aplicacion.Modelos.Respuestas;
using Aplicacion.Modelos.Solicitudes;

namespace Aplicacion.Interfaces;

public interface IServicioNota
{
    Task<ListaPaginadaRespuesta<NotaRespuesta>> ObtenerPaginadoAsync(int numeroPagina, int tamanoPagina, CancellationToken cancelacionToken);
    Task<NotaRespuesta?> ObtenerPorIdAsync(int id, CancellationToken cancelacionToken);
    Task<NotaRespuesta> CrearAsync(NotaCrearSolicitud solicitud, CancellationToken cancelacionToken);
    Task<bool> ActualizarAsync(int id, NotaActualizarSolicitud solicitud, CancellationToken cancelacionToken);
    Task<bool> EliminarAsync(int id, CancellationToken cancelacionToken);
}
