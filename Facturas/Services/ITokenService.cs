using Facturas.Models;

namespace Facturas.Services;
/// <summary>Genera tokens JWT para usuarios autenticados.</summary>
public interface ITokenService
{
    (string Token, DateTime ExpiraEn) Crear(Usuario usuario);
}