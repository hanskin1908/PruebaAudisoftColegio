using Aplicacion.Interfaces;
using Aplicacion.Modelos.Paginacion;
using Aplicacion.Modelos.Respuestas;
using Aplicacion.Modelos.Solicitudes;
using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.Interfaces;

namespace Aplicacion.Servicios;

public class ServicioProfesor : IServicioProfesor
{
    private readonly IRepositorioProfesor profesorRepositorio;
    private readonly IRepositorioConsultaNotaProfesor notaRepositorio;

    public ServicioProfesor(IRepositorioProfesor profesorRepositorio, IRepositorioConsultaNotaProfesor notaRepositorio)
    {
        this.profesorRepositorio = profesorRepositorio;
        this.notaRepositorio = notaRepositorio;
    }

    public async Task<ListaPaginadaRespuesta<ProfesorRespuesta>> ObtenerPaginadoAsync(
        int numeroPagina,
        int tamanoPagina,
        CancellationToken cancelacionToken)
    {
        var totalRegistros = await profesorRepositorio.ContarAsync(cancelacionToken);
        var profesores = await profesorRepositorio.ObtenerPaginadoAsync(numeroPagina, tamanoPagina, cancelacionToken);
        var datos = profesores.Select(MapearRespuesta).ToList();
        var totalPaginas = (int)Math.Ceiling(totalRegistros / (double)tamanoPagina);

        return new ListaPaginadaRespuesta<ProfesorRespuesta>(totalRegistros, totalPaginas, numeroPagina, datos);
    }

    public async Task<ProfesorRespuesta?> ObtenerPorIdAsync(int id, CancellationToken cancelacionToken)
    {
        var profesor = await profesorRepositorio.ObtenerPorIdAsync(id, cancelacionToken);
        return profesor == null ? null : MapearRespuesta(profesor);
    }

    public async Task<ProfesorRespuesta> CrearAsync(ProfesorCrearSolicitud solicitud, CancellationToken cancelacionToken)
    {
        var entidad = new Profesor
        {
            nombre = solicitud.nombre
        };

        var creado = await profesorRepositorio.CrearAsync(entidad, cancelacionToken);
        return MapearRespuesta(creado);
    }

    public async Task<bool> ActualizarAsync(int id, ProfesorActualizarSolicitud solicitud, CancellationToken cancelacionToken)
    {
        var entidad = await profesorRepositorio.ObtenerPorIdAsync(id, cancelacionToken);
        if (entidad == null)
        {
            return false;
        }

        entidad.nombre = solicitud.nombre;
        await profesorRepositorio.ActualizarAsync(entidad, cancelacionToken);
        return true;
    }

    public async Task<bool> EliminarAsync(int id, CancellationToken cancelacionToken)
    {
        var entidad = await profesorRepositorio.ObtenerPorIdAsync(id, cancelacionToken);
        if (entidad == null)
        {
            return false;
        }

        var tieneNotas = await notaRepositorio.ExisteParaProfesorAsync(id, cancelacionToken);
        if (tieneNotas)
        {
            throw new ExcepcionReglaNegocio("No se puede eliminar el profesor porque tiene notas asociadas.");
        }

        await profesorRepositorio.EliminarAsync(entidad, cancelacionToken);
        return true;
    }

    private static ProfesorRespuesta MapearRespuesta(Profesor profesor)
    {
        return new ProfesorRespuesta
        {
            id = profesor.id,
            nombre = profesor.nombre
        };
    }
}
