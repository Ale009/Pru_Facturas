using System.ComponentModel.DataAnnotations;

namespace Facturas.Dtos;

/// <summary>Datos para anular una factura. El motivo es obligatorio para la trazabilidad.</summary>
public class AnularFacturaRequest
{
    [Required, StringLength(200, MinimumLength = 5)]
    public string Motivo { get; set; } = string.Empty;
}