using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Facturas.Models;
using Microsoft.IdentityModel.Tokens;

namespace Facturas.Services;
/// <summary>Crea JWT firmados con HS256. La clave viene de configuración y no del código.</summary>

public class TokenService : ITokenService
{
    private readonly SigningCredentials _credenciales;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _expiraMinutos;

    public TokenService(IConfiguration config)
    {
        var clave = config["Jwt:Key"];
        // Falla al arrancar si la clave no existe o es débil, en vez de fallar en el primer login.
        if (string.IsNullOrWhiteSpace(clave) || clave.Length < 32)
            throw new InvalidOperationException("Jwt:Key debe existir y tener al menos 32 caracteres.");

        _credenciales = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(clave)),
            SecurityAlgorithms.HmacSha256);

        _issuer = config["Jwt:Issuer"]!;
        _audience = config["Jwt:Audience"]!;
        _expiraMinutos = config.GetValue<int>("Jwt:ExpiraMinutos");
    }

    public (string Token, DateTime ExpiraEn) Crear(Usuario usuario)
    {
        var expira = DateTime.UtcNow.AddMinutes(_expiraMinutos);

        // El payload del JWT va firmado, no cifrado, se recomienda no usar datos sesibles
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, usuario.NombreUsuario),
            // jti: identificador único del token
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: expira,
            signingCredentials: _credenciales);

        return (new JwtSecurityTokenHandler().WriteToken(token), expira);
    }
}