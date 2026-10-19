namespace Facturas.Models;

public class Factura
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; } = null!;
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;
    public decimal Total { get; set; }
    public EstadoFactura Estado { get; set; } = EstadoFactura.Emitida;
    public DateTime? FechaAnulacion { get; set; }
    public string? MotivoAnulacion { get; set; }
    public List<FacturaDetalle> Detalles { get; set; } = new();
}