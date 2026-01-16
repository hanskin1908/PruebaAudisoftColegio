using Microsoft.AspNetCore.Mvc;
using Presentacion.Modelos;
using Presentacion.Modelos.Respuestas;
using Presentacion.Modelos.Solicitudes;
using Presentacion.Servicios;

namespace Presentacion.Controllers;

public class ProfesoresController : Controller
{
    private readonly IServicioApiProfesor profesorApiServicio;

    public ProfesoresController(IServicioApiProfesor profesorApiServicio)
    {
        this.profesorApiServicio = profesorApiServicio;
    }

    public async Task<IActionResult> Listado(int numeroPagina = 1, int tamanoPagina = 10, CancellationToken cancelacionToken = default)
    {
        var modelo = new ListadoPaginadoVista<ProfesorRespuesta> { tamanoPagina = tamanoPagina };

        try
        {
            modelo.lista = await profesorApiServicio.ObtenerPaginadoAsync(numeroPagina, tamanoPagina, cancelacionToken);
        }
        catch (Exception ex)
        {
            modelo.mensajeError = ex.Message;
        }

        return View(modelo);
    }

    public IActionResult Crear()
    {
        return View(new ProfesorCrearSolicitud());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(ProfesorCrearSolicitud solicitud, CancellationToken cancelacionToken)
    {
        if (!ModelState.IsValid)
        {
            return View(solicitud);
        }

        var error = await profesorApiServicio.CrearAsync(solicitud, cancelacionToken);
        if (error != null)
        {
            ModelState.AddModelError(string.Empty, error);
            return View(solicitud);
        }

        TempData["MensajeExito"] = "Profesor creado correctamente.";
        return RedirectToAction(nameof(Listado));
    }

    public async Task<IActionResult> Editar(int id, CancellationToken cancelacionToken)
    {
        var profesor = await profesorApiServicio.ObtenerPorIdAsync(id, cancelacionToken);
        if (profesor == null)
        {
            return NotFound();
        }

        ViewBag.Id = id;
        return View(new ProfesorActualizarSolicitud { nombre = profesor.nombre });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, ProfesorActualizarSolicitud solicitud, CancellationToken cancelacionToken)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Id = id;
            return View(solicitud);
        }

        var error = await profesorApiServicio.ActualizarAsync(id, solicitud, cancelacionToken);
        if (error != null)
        {
            ModelState.AddModelError(string.Empty, error);
            ViewBag.Id = id;
            return View(solicitud);
        }

        TempData["MensajeExito"] = "Profesor actualizado correctamente.";
        return RedirectToAction(nameof(Listado));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Eliminar(int id, CancellationToken cancelacionToken)
    {
        var error = await profesorApiServicio.EliminarAsync(id, cancelacionToken);
        if (error != null)
        {
            TempData["MensajeError"] = error;
        }
        else
        {
            TempData["MensajeExito"] = "Profesor eliminado correctamente.";
        }

        return RedirectToAction(nameof(Listado));
    }
}
