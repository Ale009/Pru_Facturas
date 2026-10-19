using Facturas.Dtos;

namespace Facturas.Services;

/// <summary>Casos de uso de autenticación.</summary>
/// <summary>Valida las credenciales, devuelve null cuando son incorrectas</summary>
public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request);
}