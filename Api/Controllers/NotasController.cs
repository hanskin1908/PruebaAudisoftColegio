using Api.Modelos;
using Aplicacion.Interfaces;
using Aplicacion.Modelos.Paginacion;
using Aplicacion.Modelos.Respuestas;
using Aplicacion.Modelos.Solicitudes;
using Dominio.Excepciones;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/notas")]
public class NotasController : ControllerBase
{
    private readonly IServicioNota notaServicio;

    public NotasController(IServicioNota notaServicio)
    {
        this.notaServicio = notaServicio;
    }

    [HttpGet]
    public async Task<ActionResult<ListaPaginadaRespuesta<NotaRespuesta>>> ObtenerPaginado(
        [FromQuery(Name = "pageNumber")] int numeroPagina = 1,
        [FromQuery(Name = "pageSize")] int tamanoPagina = 10,
        CancellationToken cancelacionToken = default)
    {
        if (numeroPagina <= 0 || tamanoPagina <= 0)
        {
            return BadRequest(new RespuestaError { mensaje = "pageNumber y pageSize deben ser mayores a cero." });
        }

        var respuesta = await notaServicio.ObtenerPaginadoAsync(numeroPagina, tamanoPagina, cancelacionToken);
        return Ok(respuesta);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<NotaRespuesta>> ObtenerPorId(int id, CancellationToken cancelacionToken)
    {
        var nota = await notaServicio.ObtenerPorIdAsync(id, cancelacionToken);
        if (nota == null)
        {
            return NotFound();
        }

        return Ok(nota);
    }

    [HttpPost]
    public async Task<ActionResult<NotaRespuesta>> Crear(
        [FromBody] NotaCrearSolicitud solicitud,
        CancellationToken cancelacionToken)
    {
        try
        {
            var creada = await notaServicio.CrearAsync(solicitud, cancelacionToken);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = creada.id }, creada);
        }
        catch (ExcepcionReglaNegocio ex)
        {
            return BadRequest(new RespuestaError { mensaje = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(
        int id,
        [FromBody] NotaActualizarSolicitud solicitud,
        CancellationToken cancelacionToken)
    {
        try
        {
            var actualizado = await notaServicio.ActualizarAsync(id, solicitud, cancelacionToken);
            if (!actualizado)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (ExcepcionReglaNegocio ex)
        {
            return BadRequest(new RespuestaError { mensaje = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id, CancellationToken cancelacionToken)
    {
        var eliminado = await notaServicio.EliminarAsync(id, cancelacionToken);
        if (!eliminado)
        {
            return NotFound();
        }

        return NoContent();
    }
}
