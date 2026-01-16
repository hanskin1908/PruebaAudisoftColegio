using System.ComponentModel.DataAnnotations;

namespace Presentacion.Modelos.Solicitudes;

public class EstudianteActualizarSolicitud
{
    [Required]
    [MaxLength(200)]
    public string nombre { get; set; } = string.Empty;
}
