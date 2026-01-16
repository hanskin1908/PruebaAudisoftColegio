using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Presentacion.Modelos.Paginacion;
using Presentacion.Modelos.Respuestas;
using Presentacion.Modelos.Solicitudes;

namespace Presentacion.Servicios;

public class ServicioApiNota : IServicioApiNota
{
    private readonly HttpClient clienteHttp;
    private readonly JsonSerializerOptions opcionesJson = new() { PropertyNameCaseInsensitive = true };

    public ServicioApiNota(HttpClient clienteHttp)
    {
        this.clienteHttp = clienteHttp;
    }

    public async Task<ListaPaginadaRespuesta<NotaRespuesta>> ObtenerPaginadoAsync(
        int numeroPagina,
        int tamanoPagina,
        CancellationToken cancelacionToken)
    {
        var respuesta = await clienteHttp.GetAsync($"api/notas?pageNumber={numeroPagina}&pageSize={tamanoPagina}", cancelacionToken);
        if (!respuesta.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(await ObtenerMensajeErrorAsync(respuesta));
        }

        var contenido = await respuesta.Content.ReadAsStringAsync(cancelacionToken);
        return JsonSerializer.Deserialize<ListaPaginadaRespuesta<NotaRespuesta>>(contenido, opcionesJson)
            ?? new ListaPaginadaRespuesta<NotaRespuesta>();
    }

    public async Task<NotaRespuesta?> ObtenerPorIdAsync(int id, CancellationToken cancelacionToken)
    {
        var respuesta = await clienteHttp.GetAsync($"api/notas/{id}", cancelacionToken);
        if (respuesta.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        if (!respuesta.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(await ObtenerMensajeErrorAsync(respuesta));
        }

        var contenido = await respuesta.Content.ReadAsStringAsync(cancelacionToken);
        return JsonSerializer.Deserialize<NotaRespuesta>(contenido, opcionesJson);
    }

    public async Task<string?> CrearAsync(NotaCrearSolicitud solicitud, CancellationToken cancelacionToken)
    {
        var respuesta = await clienteHttp.PostAsJsonAsync("api/notas", solicitud, opcionesJson, cancelacionToken);
        if (respuesta.IsSuccessStatusCode)
        {
            return null;
        }

        return await ObtenerMensajeErrorAsync(respuesta);
    }

    public async Task<string?> ActualizarAsync(int id, NotaActualizarSolicitud solicitud, CancellationToken cancelacionToken)
    {
        var respuesta = await clienteHttp.PutAsJsonAsync($"api/notas/{id}", solicitud, opcionesJson, cancelacionToken);
        if (respuesta.IsSuccessStatusCode)
        {
            return null;
        }

        if (respuesta.StatusCode == HttpStatusCode.NotFound)
        {
            return "La nota no existe.";
        }

        return await ObtenerMensajeErrorAsync(respuesta);
    }

    public async Task<string?> EliminarAsync(int id, CancellationToken cancelacionToken)
    {
        var respuesta = await clienteHttp.DeleteAsync($"api/notas/{id}", cancelacionToken);
        if (respuesta.IsSuccessStatusCode)
        {
            return null;
        }

        if (respuesta.StatusCode == HttpStatusCode.NotFound)
        {
            return "La nota no existe.";
        }

        return await ObtenerMensajeErrorAsync(respuesta);
    }

    private async Task<string> ObtenerMensajeErrorAsync(HttpResponseMessage respuesta)
    {
        var contenido = await respuesta.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(contenido))
        {
            return $"Error HTTP {(int)respuesta.StatusCode}.";
        }

        try
        {
            var error = JsonSerializer.Deserialize<RespuestaError>(contenido, opcionesJson);
            if (!string.IsNullOrWhiteSpace(error?.mensaje))
            {
                return error.mensaje;
            }
        }
        catch (JsonException)
        {
        }

        return contenido;
    }
}
