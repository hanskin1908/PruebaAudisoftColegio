using Aplicacion.Interfaces;
using Aplicacion.Modelos.Paginacion;
using Aplicacion.Modelos.Respuestas;
using Aplicacion.Modelos.Solicitudes;
using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.Interfaces;

namespace Aplicacion.Servicios;

public class ServicioEstudiante : IServicioEstudiante
{
    private readonly IRepositorioEstudiante estudianteRepositorio;
    private readonly IRepositorioConsultaNotaEstudiante notaRepositorio;

    public ServicioEstudiante(IRepositorioEstudiante estudianteRepositorio, IRepositorioConsultaNotaEstudiante notaRepositorio)
    {
        this.estudianteRepositorio = estudianteRepositorio;
        this.notaRepositorio = notaRepositorio;
    }

    public async Task<ListaPaginadaRespuesta<EstudianteRespuesta>> ObtenerPaginadoAsync(
        int numeroPagina,
        int tamanoPagina,
        CancellationToken cancelacionToken)
    {
        var totalRegistros = await estudianteRepositorio.ContarAsync(cancelacionToken);
        var estudiantes = await estudianteRepositorio.ObtenerPaginadoAsync(numeroPagina, tamanoPagina, cancelacionToken);
        var datos = estudiantes.Select(MapearRespuesta).ToList();
        var totalPaginas = (int)Math.Ceiling(totalRegistros / (double)tamanoPagina);

        return new ListaPaginadaRespuesta<EstudianteRespuesta>(totalRegistros, totalPaginas, numeroPagina, datos);
    }

    public async Task<EstudianteRespuesta?> ObtenerPorIdAsync(int id, CancellationToken cancelacionToken)
    {
        var estudiante = await estudianteRepositorio.ObtenerPorIdAsync(id, cancelacionToken);
        return estudiante == null ? null : MapearRespuesta(estudiante);
    }

    public async Task<EstudianteRespuesta> CrearAsync(EstudianteCrearSolicitud solicitud, CancellationToken cancelacionToken)
    {
        var entidad = new Estudiante
        {
            nombre = solicitud.nombre
        };

        var creado = await estudianteRepositorio.CrearAsync(entidad, cancelacionToken);
        return MapearRespuesta(creado);
    }

    public async Task<bool> ActualizarAsync(int id, EstudianteActualizarSolicitud solicitud, CancellationToken cancelacionToken)
    {
        var entidad = await estudianteRepositorio.ObtenerPorIdAsync(id, cancelacionToken);
        if (entidad == null)
        {
            return false;
        }

        entidad.nombre = solicitud.nombre;
        await estudianteRepositorio.ActualizarAsync(entidad, cancelacionToken);
        return true;
    }

    public async Task<bool> EliminarAsync(int id, CancellationToken cancelacionToken)
    {
        var entidad = await estudianteRepositorio.ObtenerPorIdAsync(id, cancelacionToken);
        if (entidad == null)
        {
            return false;
        }

        var tieneNotas = await notaRepositorio.ExisteParaEstudianteAsync(id, cancelacionToken);
        if (tieneNotas)
        {
            throw new ExcepcionReglaNegocio("No se puede eliminar el estudiante porque tiene notas asociadas.");
        }

        await estudianteRepositorio.EliminarAsync(entidad, cancelacionToken);
        return true;
    }

    private static EstudianteRespuesta MapearRespuesta(Estudiante estudiante)
    {
        return new EstudianteRespuesta
        {
            id = estudiante.id,
            nombre = estudiante.nombre
        };
    }
}
