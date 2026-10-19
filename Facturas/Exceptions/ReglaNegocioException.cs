namespace Facturas.Exceptions;

/// <summary>Se lanza cuando una operación válida en formato incumple una regla del negocio.</summary>
public class ReglaNegocioException : Exception
{
    public ReglaNegocioException(string mensaje) : base(mensaje) { }
}