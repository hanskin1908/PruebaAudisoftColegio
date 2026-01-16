using System.Text.Json.Serialization;

namespace Aplicacion.Modelos.Paginacion;

public class ListaPaginadaRespuesta<T>
{
    [JsonPropertyName("totalRecords")]
    public int totalRegistros { get; init; }

    [JsonPropertyName("totalPages")]
    public int totalPaginas { get; init; }

    [JsonPropertyName("currentPage")]
    public int paginaActual { get; init; }

    [JsonPropertyName("data")]
    public IReadOnlyList<T> datos { get; init; }

    public ListaPaginadaRespuesta(int totalRegistros, int totalPaginas, int paginaActual, IReadOnlyList<T> datos)
    {
        this.totalRegistros = totalRegistros;
        this.totalPaginas = totalPaginas;
        this.paginaActual = paginaActual;
        this.datos = datos;
    }
}
