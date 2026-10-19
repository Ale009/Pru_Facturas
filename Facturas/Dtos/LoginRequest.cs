using System.ComponentModel.DataAnnotations;

namespace Facturas.Dtos;

public class LoginRequest
{
    [Required, StringLength(50)]
    public string Usuario { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Password { get; set; } = string.Empty;
}