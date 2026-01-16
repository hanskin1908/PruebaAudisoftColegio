using Api.Modelos;
using Aplicacion.Interfaces;
using Aplicacion.Modelos.Paginacion;
using Aplicacion.Modelos.Respuestas;
using Aplicacion.Modelos.Solicitudes;
using Dominio.Excepciones;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/profesores")]
public class ProfesoresController : ControllerBase
{
    private readonly IServicioProfesor profesorServicio;

    public ProfesoresController(IServicioProfesor profesorServicio)
    {
        this.profesorServicio = profesorServicio;
    }

    [HttpGet]
    public async Task<ActionResult<ListaPaginadaRespuesta<ProfesorRespuesta>>> ObtenerPaginado(
        [FromQuery(Name = "pageNumber")] int numeroPagina = 1,
        [FromQuery(Name = "pageSize")] int tamanoPagina = 10,
        CancellationToken cancelacionToken = default)
    {
        if (numeroPagina <= 0 || tamanoPagina <= 0)
        {
            return BadRequest(new RespuestaError { mensaje = "pageNumber y pageSize deben ser mayores a cero." });
        }

        var respuesta = await profesorServicio.ObtenerPaginadoAsync(numeroPagina, tamanoPagina, cancelacionToken);
        return Ok(respuesta);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProfesorRespuesta>> ObtenerPorId(int id, CancellationToken cancelacionToken)
    {
        var profesor = await profesorServicio.ObtenerPorIdAsync(id, cancelacionToken);
        if (profesor == null)
        {
            return NotFound();
        }

        return Ok(profesor);
    }

    [HttpPost]
    public async Task<ActionResult<ProfesorRespuesta>> Crear(
        [FromBody] ProfesorCrearSolicitud solicitud,
        CancellationToken cancelacionToken)
    {
        var creado = await profesorServicio.CrearAsync(solicitud, cancelacionToken);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.id }, creado);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(
        int id,
        [FromBody] ProfesorActualizarSolicitud solicitud,
        CancellationToken cancelacionToken)
    {
        var actualizado = await profesorServicio.ActualizarAsync(id, solicitud, cancelacionToken);
        if (!actualizado)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id, CancellationToken cancelacionToken)
    {
        try
        {
            var eliminado = await profesorServicio.EliminarAsync(id, cancelacionToken);
            if (!eliminado)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (ExcepcionReglaNegocio ex)
        {
            return Conflict(new RespuestaError { mensaje = ex.Message });
        }
    }
}
