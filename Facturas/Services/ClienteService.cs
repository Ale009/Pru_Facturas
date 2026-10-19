using Facturas.Data;
using Facturas.Dtos;
using Facturas.Models;
using Microsoft.EntityFrameworkCore;

namespace Facturas.Services;

/// <summary>Mantenimiento del catálogo de clientes. Nunca elimina registros: los desactiva.</summary>
public class ClienteService : IClienteService
{
    private readonly AppDbContext _db;

    public ClienteService(AppDbContext db) => _db = db;

    public async Task<IReadOnlyList<ClienteResponse>> ListarAsync(bool soloActivos)
    {
        var consulta = _db.Clientes.AsNoTracking();

        if (soloActivos)
            consulta = consulta.Where(c => c.Activo);

        var clientes = await consulta.OrderBy(c => c.Nombre).ToListAsync();
        return clientes.Select(Mapear).ToList();
    }

    public async Task<ClienteResponse?> ObtenerAsync(int id)
    {
        var cliente = await _db.Clientes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        return cliente is null ? null : Mapear(cliente);
    }

    public async Task<ClienteResponse> CrearAsync(ClienteRequest request)
    {
        var cliente = new Cliente
        {
            Nombre = request.Nombre.Trim(),
            Nit = request.Nit.Trim(),
            Direccion = request.Direccion?.Trim()
        };

        _db.Clientes.Add(cliente);
        await _db.SaveChangesAsync();
        return Mapear(cliente);
    }

    public async Task<ClienteResponse?> ActualizarAsync(int id, ClienteRequest request)
    {
        var cliente = await _db.Clientes.FindAsync(id);
        if (cliente is null) return null;

        cliente.Nombre = request.Nombre.Trim();
        cliente.Nit = request.Nit.Trim();
        cliente.Direccion = request.Direccion?.Trim();

        await _db.SaveChangesAsync();
        return Mapear(cliente);
    }

    // Borrado lógico: el cliente puede tener facturas asociadas y debe conservarse.
    public async Task<bool> DesactivarAsync(int id)
    {
        var cliente = await _db.Clientes.FindAsync(id);
        if (cliente is null) return false;

        cliente.Activo = false;
        await _db.SaveChangesAsync();
        return true;
    }

    private static ClienteResponse Mapear(Cliente c) =>
        new(c.Id, c.Nombre, c.Nit, c.Direccion, c.Activo);
}