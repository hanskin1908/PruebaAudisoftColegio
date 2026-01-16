using Microsoft.AspNetCore.Mvc;
using Presentacion.Modelos;
using Presentacion.Modelos.Paginacion;
using Presentacion.Modelos.Respuestas;
using Presentacion.Modelos.Solicitudes;
using Presentacion.Servicios;

namespace Presentacion.Controllers;

public class EstudiantesController : Controller
{
    private readonly IServicioApiEstudiante estudianteApiServicio;

    public EstudiantesController(IServicioApiEstudiante estudianteApiServicio)
    {
        this.estudianteApiServicio = estudianteApiServicio;
    }

    public async Task<IActionResult> Listado(int numeroPagina = 1, int tamanoPagina = 10, CancellationToken cancelacionToken = default)
    {
        var modelo = new ListadoPaginadoVista<EstudianteRespuesta> { tamanoPagina = tamanoPagina };

        try
        {
            modelo.lista = await estudianteApiServicio.ObtenerPaginadoAsync(numeroPagina, tamanoPagina, cancelacionToken);
        }
        catch (Exception ex)
        {
            modelo.mensajeError = ex.Message;
        }

        return View(modelo);
    }

    public IActionResult Crear()
    {
        return View(new EstudianteCrearSolicitud());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(EstudianteCrearSolicitud solicitud, CancellationToken cancelacionToken)
    {
        if (!ModelState.IsValid)
        {
            return View(solicitud);
        }

        var error = await estudianteApiServicio.CrearAsync(solicitud, cancelacionToken);
        if (error != null)
        {
            ModelState.AddModelError(string.Empty, error);
            return View(solicitud);
        }

        TempData["MensajeExito"] = "Estudiante creado correctamente.";
        return RedirectToAction(nameof(Listado));
    }

    public async Task<IActionResult> Editar(int id, CancellationToken cancelacionToken)
    {
        var estudiante = await estudianteApiServicio.ObtenerPorIdAsync(id, cancelacionToken);
        if (estudiante == null)
        {
            return NotFound();
        }

        ViewBag.Id = id;
        return View(new EstudianteActualizarSolicitud { nombre = estudiante.nombre });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, EstudianteActualizarSolicitud solicitud, CancellationToken cancelacionToken)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Id = id;
            return View(solicitud);
        }

        var error = await estudianteApiServicio.ActualizarAsync(id, solicitud, cancelacionToken);
        if (error != null)
        {
            ModelState.AddModelError(string.Empty, error);
            ViewBag.Id = id;
            return View(solicitud);
        }

        TempData["MensajeExito"] = "Estudiante actualizado correctamente.";
        return RedirectToAction(nameof(Listado));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Eliminar(int id, CancellationToken cancelacionToken)
    {
        var error = await estudianteApiServicio.EliminarAsync(id, cancelacionToken);
        if (error != null)
        {
            TempData["MensajeError"] = error;
        }
        else
        {
            TempData["MensajeExito"] = "Estudiante eliminado correctamente.";
        }

        return RedirectToAction(nameof(Listado));
    }
}
