using Presentacion.Modelos.Paginacion;
using Presentacion.Modelos.Respuestas;

namespace Presentacion.Servicios;

public interface IServicioApiConsultaProfesor
{
    Task<ListaPaginadaRespuesta<ProfesorRespuesta>> ObtenerPaginadoAsync(int numeroPagina, int tamanoPagina, CancellationToken cancelacionToken);
    Task<ProfesorRespuesta?> ObtenerPorIdAsync(int id, CancellationToken cancelacionToken);
}
