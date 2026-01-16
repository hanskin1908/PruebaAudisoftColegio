using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Presentacion.Modelos.Paginacion;
using Presentacion.Modelos.Respuestas;
using Presentacion.Modelos.Solicitudes;

namespace Presentacion.Servicios;

public class ServicioApiProfesor : IServicioApiProfesor, IServicioApiConsultaProfesor
{
    private readonly HttpClient clienteHttp;
    private readonly JsonSerializerOptions opcionesJson = new() { PropertyNameCaseInsensitive = true };

    public ServicioApiProfesor(HttpClient clienteHttp)
    {
        this.clienteHttp = clienteHttp;
    }

    public async Task<ListaPaginadaRespuesta<ProfesorRespuesta>> ObtenerPaginadoAsync(
        int numeroPagina,
        int tamanoPagina,
        CancellationToken cancelacionToken)
    {
        var respuesta = await clienteHttp.GetAsync($"api/profesores?pageNumber={numeroPagina}&pageSize={tamanoPagina}", cancelacionToken);
        if (!respuesta.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(await ObtenerMensajeErrorAsync(respuesta));
        }

        var contenido = await respuesta.Content.ReadAsStringAsync(cancelacionToken);
        return JsonSerializer.Deserialize<ListaPaginadaRespuesta<ProfesorRespuesta>>(contenido, opcionesJson)
            ?? new ListaPaginadaRespuesta<ProfesorRespuesta>();
    }

    public async Task<ProfesorRespuesta?> ObtenerPorIdAsync(int id, CancellationToken cancelacionToken)
    {
        var respuesta = await clienteHttp.GetAsync($"api/profesores/{id}", cancelacionToken);
        if (respuesta.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        if (!respuesta.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(await ObtenerMensajeErrorAsync(respuesta));
        }

        var contenido = await respuesta.Content.ReadAsStringAsync(cancelacionToken);
        return JsonSerializer.Deserialize<ProfesorRespuesta>(contenido, opcionesJson);
    }

    public async Task<string?> CrearAsync(ProfesorCrearSolicitud solicitud, CancellationToken cancelacionToken)
    {
        var respuesta = await clienteHttp.PostAsJsonAsync("api/profesores", solicitud, opcionesJson, cancelacionToken);
        if (respuesta.IsSuccessStatusCode)
        {
            return null;
        }

        return await ObtenerMensajeErrorAsync(respuesta);
    }

    public async Task<string?> ActualizarAsync(int id, ProfesorActualizarSolicitud solicitud, CancellationToken cancelacionToken)
    {
        var respuesta = await clienteHttp.PutAsJsonAsync($"api/profesores/{id}", solicitud, opcionesJson, cancelacionToken);
        if (respuesta.IsSuccessStatusCode)
        {
            return null;
        }

        if (respuesta.StatusCode == HttpStatusCode.NotFound)
        {
            return "El profesor no existe.";
        }

        return await ObtenerMensajeErrorAsync(respuesta);
    }

    public async Task<string?> EliminarAsync(int id, CancellationToken cancelacionToken)
    {
        var respuesta = await clienteHttp.DeleteAsync($"api/profesores/{id}", cancelacionToken);
        if (respuesta.IsSuccessStatusCode)
        {
            return null;
        }

        if (respuesta.StatusCode == HttpStatusCode.NotFound)
        {
            return "El profesor no existe.";
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
