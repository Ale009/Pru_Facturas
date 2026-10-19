using System.ComponentModel.DataAnnotations;

namespace Facturas.Dtos;

/// <summary>Datos para emitir una factura. El precio viene del servidor.</summary>
public class FacturaCrearRequest
{
    [Range(1, int.MaxValue)]
    public int ClienteId { get; set; }

    [Required, MinLength(1, ErrorMessage = "La factura debe tener detalle")]
    public List<FacturaDetalleRequest> Detalles { get; set; } = new();
}

/// <summary>Línea solicitada: qué producto y cuántas unidades.</summary>
public class FacturaDetalleRequest
{
    [Range(1, int.MaxValue)]
    public int ProductoId { get; set; }

    [Range(1, 10000)]
    public int Cantidad { get; set; }
}