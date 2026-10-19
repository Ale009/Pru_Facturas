namespace Facturas.Models;

public class Cliente
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Nit { get; set; } = "CF";
    public string? Direccion { get; set; }
    public bool Activo { get; set; } = true;
}