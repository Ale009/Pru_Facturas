using Facturas.Dtos;

namespace Facturas.Services;

/// <summary>Casos de uso del mantenimiento de productos.</summary>
public interface IProductoService
{
    Task<IReadOnlyList<ProductoResponse>> ListarAsync(bool soloActivos);
    Task<ProductoResponse?> ObtenerAsync(int id);
    Task<ProductoResponse> CrearAsync(ProductoRequest request);
    Task<ProductoResponse?> ActualizarAsync(int id, ProductoRequest request);
    Task<bool> DesactivarAsync(int id);
}