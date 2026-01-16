using System.ComponentModel.DataAnnotations;

namespace Aplicacion.Modelos.Solicitudes;

public class NotaActualizarSolicitud
{
    [Required]
    [MaxLength(200)]
    public string nombre { get; set; } = string.Empty;
    public decimal valor { get; set; }
    public int idProfesor { get; set; }
    public int idEstudiante { get; set; }
}
