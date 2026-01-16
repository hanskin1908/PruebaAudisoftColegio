using System.ComponentModel.DataAnnotations;

namespace Presentacion.Modelos.Solicitudes;

public class NotaCrearSolicitud
{
    [Required]
    [MaxLength(200)]
    public string nombre { get; set; } = string.Empty;
    public decimal valor { get; set; }
    [Display(Name = "Profesor")]
    public int idProfesor { get; set; }
    [Display(Name = "Estudiante")]
    public int idEstudiante { get; set; }
}
