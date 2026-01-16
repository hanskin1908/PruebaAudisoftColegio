using Api.Modelos;
using Aplicacion.Interfaces;
using Aplicacion.Modelos.Paginacion;
using Aplicacion.Modelos.Respuestas;
using Aplicacion.Modelos.Solicitudes;
using Dominio.Excepciones;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/estudiantes")]
public class EstudiantesController : ControllerBase
{
    private readonly IServicioEstudiante estudianteServicio;

    public EstudiantesController(IServicioEstudiante estudianteServicio)
    {
        this.estudianteServicio = estudianteServicio;
    }

    [HttpGet]
    public async Task<ActionResult<ListaPaginadaRespuesta<EstudianteRespuesta>>> ObtenerPaginado(
        [FromQuery(Name = "pageNumber")] int numeroPagina = 1,
        [FromQuery(Name = "pageSize")] int tamanoPagina = 10,
        CancellationToken cancelacionToken = default)
    {
        if (numeroPagina <= 0 || tamanoPagina <= 0)
        {
            return BadRequest(new RespuestaError { mensaje = "pageNumber y pageSize deben ser mayores a cero." });
        }

        var respuesta = await estudianteServicio.ObtenerPaginadoAsync(numeroPagina, tamanoPagina, cancelacionToken);
        return Ok(respuesta);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EstudianteRespuesta>> ObtenerPorId(int id, CancellationToken cancelacionToken)
    {
        var estudiante = await estudianteServicio.ObtenerPorIdAsync(id, cancelacionToken);
        if (estudiante == null)
        {
            return NotFound();
        }

        return Ok(estudiante);
    }

    [HttpPost]
    public async Task<ActionResult<EstudianteRespuesta>> Crear(
        [FromBody] EstudianteCrearSolicitud solicitud,
        CancellationToken cancelacionToken)
    {
        var creado = await estudianteServicio.CrearAsync(solicitud, cancelacionToken);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.id }, creado);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(
        int id,
        [FromBody] EstudianteActualizarSolicitud solicitud,
        CancellationToken cancelacionToken)
    {
        var actualizado = await estudianteServicio.ActualizarAsync(id, solicitud, cancelacionToken);
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
            var eliminado = await estudianteServicio.EliminarAsync(id, cancelacionToken);
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
