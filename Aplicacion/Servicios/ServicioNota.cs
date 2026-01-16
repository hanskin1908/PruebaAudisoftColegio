using Aplicacion.Interfaces;
using Aplicacion.Modelos.Paginacion;
using Aplicacion.Modelos.Respuestas;
using Aplicacion.Modelos.Solicitudes;
using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.Interfaces;

namespace Aplicacion.Servicios;

public class ServicioNota : IServicioNota
{
    private readonly IRepositorioNota notaRepositorio;
    private readonly IRepositorioExistenciaEstudiante estudianteRepositorio;
    private readonly IRepositorioExistenciaProfesor profesorRepositorio;

    public ServicioNota(
        IRepositorioNota notaRepositorio,
        IRepositorioExistenciaEstudiante estudianteRepositorio,
        IRepositorioExistenciaProfesor profesorRepositorio)
    {
        this.notaRepositorio = notaRepositorio;
        this.estudianteRepositorio = estudianteRepositorio;
        this.profesorRepositorio = profesorRepositorio;
    }

    public async Task<ListaPaginadaRespuesta<NotaRespuesta>> ObtenerPaginadoAsync(
        int numeroPagina,
        int tamanoPagina,
        CancellationToken cancelacionToken)
    {
        var totalRegistros = await notaRepositorio.ContarAsync(cancelacionToken);
        var notas = await notaRepositorio.ObtenerPaginadoAsync(numeroPagina, tamanoPagina, cancelacionToken);
        var datos = notas.Select(MapearRespuesta).ToList();
        var totalPaginas = (int)Math.Ceiling(totalRegistros / (double)tamanoPagina);

        return new ListaPaginadaRespuesta<NotaRespuesta>(totalRegistros, totalPaginas, numeroPagina, datos);
    }

    public async Task<NotaRespuesta?> ObtenerPorIdAsync(int id, CancellationToken cancelacionToken)
    {
        var nota = await notaRepositorio.ObtenerPorIdAsync(id, cancelacionToken);
        return nota == null ? null : MapearRespuesta(nota);
    }

    public async Task<NotaRespuesta> CrearAsync(NotaCrearSolicitud solicitud, CancellationToken cancelacionToken)
    {
        await ValidarReferenciasAsync(solicitud.idEstudiante, solicitud.idProfesor, cancelacionToken);

        var entidad = new Nota
        {
            nombre = solicitud.nombre,
            valor = solicitud.valor,
            idEstudiante = solicitud.idEstudiante,
            idProfesor = solicitud.idProfesor
        };

        var creada = await notaRepositorio.CrearAsync(entidad, cancelacionToken);
        return MapearRespuesta(creada);
    }

    public async Task<bool> ActualizarAsync(int id, NotaActualizarSolicitud solicitud, CancellationToken cancelacionToken)
    {
        var entidad = await notaRepositorio.ObtenerPorIdAsync(id, cancelacionToken);
        if (entidad == null)
        {
            return false;
        }

        await ValidarReferenciasAsync(solicitud.idEstudiante, solicitud.idProfesor, cancelacionToken);

        entidad.nombre = solicitud.nombre;
        entidad.valor = solicitud.valor;
        entidad.idEstudiante = solicitud.idEstudiante;
        entidad.idProfesor = solicitud.idProfesor;

        await notaRepositorio.ActualizarAsync(entidad, cancelacionToken);
        return true;
    }

    public async Task<bool> EliminarAsync(int id, CancellationToken cancelacionToken)
    {
        var entidad = await notaRepositorio.ObtenerPorIdAsync(id, cancelacionToken);
        if (entidad == null)
        {
            return false;
        }

        await notaRepositorio.EliminarAsync(entidad, cancelacionToken);
        return true;
    }

    private async Task ValidarReferenciasAsync(int idEstudiante, int idProfesor, CancellationToken cancelacionToken)
    {
        var existeEstudiante = await estudianteRepositorio.ExisteAsync(idEstudiante, cancelacionToken);
        if (!existeEstudiante)
        {
            throw new ExcepcionReglaNegocio("El estudiante especificado no existe.");
        }

        var existeProfesor = await profesorRepositorio.ExisteAsync(idProfesor, cancelacionToken);
        if (!existeProfesor)
        {
            throw new ExcepcionReglaNegocio("El profesor especificado no existe.");
        }
    }

    private static NotaRespuesta MapearRespuesta(Nota nota)
    {
        return new NotaRespuesta
        {
            id = nota.id,
            nombre = nota.nombre,
            valor = nota.valor,
            idProfesor = nota.idProfesor,
            idEstudiante = nota.idEstudiante
        };
    }
}
