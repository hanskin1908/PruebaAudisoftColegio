using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Presentacion.Modelos;
using Presentacion.Modelos.Respuestas;
using Presentacion.Modelos.Solicitudes;
using Presentacion.Servicios;

namespace Presentacion.Controllers;

public class NotasController : Controller
{
    private readonly IServicioApiNota notaApiServicio;
    private readonly IServicioApiConsultaProfesor profesorApiServicio;
    private readonly IServicioApiConsultaEstudiante estudianteApiServicio;

    public NotasController(
        IServicioApiNota notaApiServicio,
        IServicioApiConsultaProfesor profesorApiServicio,
        IServicioApiConsultaEstudiante estudianteApiServicio)
    {
        this.notaApiServicio = notaApiServicio;
        this.profesorApiServicio = profesorApiServicio;
        this.estudianteApiServicio = estudianteApiServicio;
    }

    public async Task<IActionResult> Listado(int numeroPagina = 1, int tamanoPagina = 10, CancellationToken cancelacionToken = default)
    {
        var modelo = new ListadoPaginadoVista<NotaRespuesta> { tamanoPagina = tamanoPagina };

        try
        {
            modelo.lista = await notaApiServicio.ObtenerPaginadoAsync(numeroPagina, tamanoPagina, cancelacionToken);
        }
        catch (Exception ex)
        {
            modelo.mensajeError = ex.Message;
        }

        return View(modelo);
    }

    public async Task<IActionResult> Crear(CancellationToken cancelacionToken = default)
    {
        await CargarListasAsync(null, null, cancelacionToken);
        return View(new NotaCrearSolicitud());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(NotaCrearSolicitud solicitud, CancellationToken cancelacionToken)
    {
        var nombreEstudiante = await ObtenerNombreEstudianteAsync(solicitud.idEstudiante, cancelacionToken);
        if (string.IsNullOrWhiteSpace(nombreEstudiante))
        {
            ModelState.Remove(nameof(NotaCrearSolicitud.nombre));
            ModelState.AddModelError(nameof(NotaCrearSolicitud.idEstudiante), "El estudiante seleccionado no existe.");
        }
        else
        {
            solicitud.nombre = nombreEstudiante;
            ModelState.Remove(nameof(NotaCrearSolicitud.nombre));
        }

        if (!ModelState.IsValid)
        {
            await CargarListasAsync(solicitud.idProfesor, solicitud.idEstudiante, cancelacionToken);
            return View(solicitud);
        }

        var error = await notaApiServicio.CrearAsync(solicitud, cancelacionToken);
        if (error != null)
        {
            ModelState.AddModelError(string.Empty, error);
            await CargarListasAsync(solicitud.idProfesor, solicitud.idEstudiante, cancelacionToken);
            return View(solicitud);
        }

        TempData["MensajeExito"] = "Nota creada correctamente.";
        return RedirectToAction(nameof(Listado));
    }

    public async Task<IActionResult> Editar(int id, CancellationToken cancelacionToken)
    {
        var nota = await notaApiServicio.ObtenerPorIdAsync(id, cancelacionToken);
        if (nota == null)
        {
            return NotFound();
        }

        ViewBag.Id = id;
        await CargarListasAsync(nota.idProfesor, nota.idEstudiante, cancelacionToken);
        return View(new NotaActualizarSolicitud
        {
            nombre = nota.nombre,
            valor = nota.valor,
            idEstudiante = nota.idEstudiante,
            idProfesor = nota.idProfesor
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, NotaActualizarSolicitud solicitud, CancellationToken cancelacionToken)
    {
        var nombreEstudiante = await ObtenerNombreEstudianteAsync(solicitud.idEstudiante, cancelacionToken);
        if (string.IsNullOrWhiteSpace(nombreEstudiante))
        {
            ModelState.Remove(nameof(NotaActualizarSolicitud.nombre));
            ModelState.AddModelError(nameof(NotaActualizarSolicitud.idEstudiante), "El estudiante seleccionado no existe.");
        }
        else
        {
            solicitud.nombre = nombreEstudiante;
            ModelState.Remove(nameof(NotaActualizarSolicitud.nombre));
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Id = id;
            await CargarListasAsync(solicitud.idProfesor, solicitud.idEstudiante, cancelacionToken);
            return View(solicitud);
        }

        var error = await notaApiServicio.ActualizarAsync(id, solicitud, cancelacionToken);
        if (error != null)
        {
            ModelState.AddModelError(string.Empty, error);
            ViewBag.Id = id;
            await CargarListasAsync(solicitud.idProfesor, solicitud.idEstudiante, cancelacionToken);
            return View(solicitud);
        }

        TempData["MensajeExito"] = "Nota actualizada correctamente.";
        return RedirectToAction(nameof(Listado));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Eliminar(int id, CancellationToken cancelacionToken)
    {
        var error = await notaApiServicio.EliminarAsync(id, cancelacionToken);
        if (error != null)
        {
            TempData["MensajeError"] = error;
        }
        else
        {
            TempData["MensajeExito"] = "Nota eliminada correctamente.";
        }

        return RedirectToAction(nameof(Listado));
    }

    private async Task CargarListasAsync(int? idProfesorSeleccionado, int? idEstudianteSeleccionado, CancellationToken cancelacionToken)
    {
        const int tamanoPaginaListas = 1000;
        var profesores = await profesorApiServicio.ObtenerPaginadoAsync(1, tamanoPaginaListas, cancelacionToken);
        var estudiantes = await estudianteApiServicio.ObtenerPaginadoAsync(1, tamanoPaginaListas, cancelacionToken);

        ViewBag.Profesores = new SelectList(profesores.datos, nameof(ProfesorRespuesta.id), nameof(ProfesorRespuesta.nombre), idProfesorSeleccionado);
        ViewBag.Estudiantes = new SelectList(estudiantes.datos, nameof(EstudianteRespuesta.id), nameof(EstudianteRespuesta.nombre), idEstudianteSeleccionado);
    }

    private async Task<string?> ObtenerNombreEstudianteAsync(int idEstudiante, CancellationToken cancelacionToken)
    {
        var estudiante = await estudianteApiServicio.ObtenerPorIdAsync(idEstudiante, cancelacionToken);
        return estudiante?.nombre;
    }
}
