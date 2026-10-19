using Facturas.Dtos;
using Facturas.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Facturas.Controllers;

/// <summary>Endpoints de autenticación. Solo traduce HTTP; la lógica vive en IAuthService.</summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;

    public AuthController(IAuthService auth) => _auth = auth;

    /// <summary>Inicia sesión y devuelve un token JWT.</summary>
    [HttpPost("login")]
    [EnableRateLimiting("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var resultado = await _auth.LoginAsync(request);

        // Mensaje genérico: no revela si falló el usuario o la contraseña.
        if (resultado is null)
            return Problem(
                title: "Credenciales inválidas",
                detail: "Usuario o contraseña incorrectos.",
                statusCode: StatusCodes.Status401Unauthorized);

        return Ok(resultado);
    }
}