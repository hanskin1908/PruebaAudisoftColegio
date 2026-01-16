using System.Text.Json.Serialization;

namespace Presentacion.Modelos.Paginacion;

public class ListaPaginadaRespuesta<T>
{
    [JsonPropertyName("totalRecords")]
    public int totalRegistros { get; set; }

    [JsonPropertyName("totalPages")]
    public int totalPaginas { get; set; }

    [JsonPropertyName("currentPage")]
    public int paginaActual { get; set; }

    [JsonPropertyName("data")]
    public List<T> datos { get; set; } = new();
}
