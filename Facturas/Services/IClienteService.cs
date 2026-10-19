using Facturas.Dtos;

namespace Facturas.Services;

/// <summary>Casos de uso del mantenimiento de clientes.</summary>
public interface IClienteService
{
    Task<IReadOnlyList<ClienteResponse>> ListarAsync(bool soloActivos);
    Task<ClienteResponse?> ObtenerAsync(int id);
    Task<ClienteResponse> CrearAsync(ClienteRequest request);
    Task<ClienteResponse?> ActualizarAsync(int id, ClienteRequest request);
    Task<bool> DesactivarAsync(int id);
}