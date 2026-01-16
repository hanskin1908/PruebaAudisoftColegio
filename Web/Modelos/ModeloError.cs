namespace Presentacion.Modelos;

public class ModeloError
{
    public string? idSolicitud { get; set; }

    public bool MostrarIdSolicitud => !string.IsNullOrWhiteSpace(idSolicitud);
}
