namespace Presentacion.Modelos.Respuestas;

public class NotaRespuesta
{
    public int id { get; set; }
    public string nombre { get; set; } = string.Empty;
    public decimal valor { get; set; }
    public int idProfesor { get; set; }
    public int idEstudiante { get; set; }
}
