using Facturas.Dtos;
using Facturas.Models;
namespace Facturas.Services;


/// <summary>Casos de uso de facturación.</summary>
public interface IFacturaService
{
    Task<FacturaResponse> CrearAsync(FacturaCrearRequest request, int usuarioId);
    Task<FacturaResponse?> ObtenerAsync(int id);
    /// <summary>Anula una factura emitida. Devuelve null si no existe.</summary>
    Task<FacturaResponse?> AnularAsync(int id, string motivo);
    /// <summary>Lista las facturas con su valor y estado, de la más reciente a la más antigua.</summary>
    Task<IReadOnlyList<FacturaResumenResponse>> ListarAsync(EstadoFactura? estado);
}