using System.ComponentModel.DataAnnotations;

namespace Facturas.Dtos;

/// <summary>Datos que el cliente de la API envía para crear o actualizar un cliente.</summary>
public class ClienteRequest
{
    [Required, StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required, StringLength(20)]
    public string Nit { get; set; } = string.Empty;

    [StringLength(200)]
    public string? Direccion { get; set; }
}