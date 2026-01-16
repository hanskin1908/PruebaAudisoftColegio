namespace Dominio.Entidades;

public class Estudiante
{
    public int id { get; set; }
    public string nombre { get; set; } = string.Empty;
    public ICollection<Nota> notas { get; set; } = new List<Nota>();
}
