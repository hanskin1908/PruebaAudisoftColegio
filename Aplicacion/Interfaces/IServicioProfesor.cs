using Aplicacion.Modelos.Paginacion;
using Aplicacion.Modelos.Respuestas;
using Aplicacion.Modelos.Solicitudes;

namespace Aplicacion.Interfaces;

public interface IServicioProfesor
{
    Task<ListaPaginadaRespuesta<ProfesorRespuesta>> ObtenerPaginadoAsync(int numeroPagina, int tamanoPagina, CancellationToken cancelacionToken);
    Task<ProfesorRespuesta?> ObtenerPorIdAsync(int id, CancellationToken cancelacionToken);
    Task<ProfesorRespuesta> CrearAsync(ProfesorCrearSolicitud solicitud, CancellationToken cancelacionToken);
    Task<bool> ActualizarAsync(int id, ProfesorActualizarSolicitud solicitud, CancellationToken cancelacionToken);
    Task<bool> EliminarAsync(int id, CancellationToken cancelacionToken);
}
