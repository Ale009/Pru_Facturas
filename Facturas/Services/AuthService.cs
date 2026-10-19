using Facturas.Data;
using Facturas.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Facturas.Services;

/// <summary>Valida usuario y contraseña (BCrypt) y emite el token de sesión.</summary>
/// Hash de relleno para igualar el tiempo de respuesta cuando el usuario no existe.
public class AuthService : IAuthService
{
    private static readonly string HashFicticio = BCrypt.Net.BCrypt.HashPassword("relleno");
    private readonly AppDbContext _db;
    private readonly ITokenService _tokens;

    public AuthService(AppDbContext db, ITokenService tokens)
    {
        _db = db;
        _tokens = tokens;
    }
    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var usuario = await _db.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.NombreUsuario == request.Usuario && u.Activo);

        // Se verifica siempre contra un hash, si el usuario existe o no, para que el
        // tiempo de respuesta no revele qué usuarios existen (ataque de temporización).
        var hash = usuario?.PasswordHash ?? HashFicticio;
        var passwordValida = BCrypt.Net.BCrypt.Verify(request.Password, hash);

        if (usuario is null || !passwordValida)
            return null;

        var (token, expira) = _tokens.Crear(usuario);
        return new LoginResponse(token, usuario.NombreUsuario, expira);
    }
}