namespace Dominio.Entidades;

public class Nota
{
    public int id { get; set; }
    public string nombre { get; set; } = string.Empty;
    public decimal valor { get; set; }
    public int idProfesor { get; set; }
    public int idEstudiante { get; set; }
    public Profesor? profesor { get; set; }
    public Estudiante? estudiante { get; set; }
}
