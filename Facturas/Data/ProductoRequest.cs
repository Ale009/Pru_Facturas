using System.ComponentModel.DataAnnotations;

namespace Facturas.Dtos;

/// <summary>Datos para crear o actualizar un producto.</summary>
public class ProductoRequest
{
    [Required, StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Range(typeof(decimal), "0.01", "9999999.99")]
    public decimal PrecioUnitario { get; set; }
}