namespace Dominio.Excepciones;

public class ExcepcionReglaNegocio : Exception
{
    public ExcepcionReglaNegocio(string mensaje) : base(mensaje)
    {
    }
}
