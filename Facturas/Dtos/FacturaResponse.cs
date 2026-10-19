namespace Facturas.Dtos;

/// <summary>Factura completa (encabezado y detalle).</summary>
public record FacturaResponse(
    int Id,
    DateTime Fecha,
    int ClienteId,
    string Cliente,
    string Usuario,
    decimal Total,
    string Estado,
    DateTime? FechaAnulacion,
    string? MotivoAnulacion,
    IReadOnlyList<FacturaDetalleResponse> Detalles);

/// <summary>Detalle de una factura.</summary>
public record FacturaDetalleResponse(
    int ProductoId,
    string Producto,
    int Cantidad,
    decimal PrecioUnitario,
    decimal Subtotal);