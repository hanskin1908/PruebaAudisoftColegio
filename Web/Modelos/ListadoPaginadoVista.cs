using Presentacion.Modelos.Paginacion;

namespace Presentacion.Modelos;

public class ListadoPaginadoVista<T>
{
    public ListaPaginadaRespuesta<T> lista { get; set; } = new();
    public int tamanoPagina { get; set; }
    public string? mensajeError { get; set; }
}
