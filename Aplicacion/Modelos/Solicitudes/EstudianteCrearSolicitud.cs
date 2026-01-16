using System.ComponentModel.DataAnnotations;

namespace Aplicacion.Modelos.Solicitudes;

public class EstudianteCrearSolicitud
{
    [Required]
    [MaxLength(200)]
    public string nombre { get; set; } = string.Empty;
}
