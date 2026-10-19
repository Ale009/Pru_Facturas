using Facturas.Data;
using Facturas.Dtos;
using Facturas.Models;
using Microsoft.EntityFrameworkCore;

namespace Facturas.Services;

/// <summary>Mantenimiento del catálogo de productos. Nunca elimina registros: los desactiva.</summary>
public class ProductoService : IProductoService
{
    private readonly AppDbContext _db;

    public ProductoService(AppDbContext db) => _db = db;

    public async Task<IReadOnlyList<ProductoResponse>> ListarAsync(bool soloActivos)
    {
        var consulta = _db.Productos.AsNoTracking();

        if (soloActivos)
            consulta = consulta.Where(p => p.Activo);

        var productos = await consulta.OrderBy(p => p.Nombre).ToListAsync();
        return productos.Select(Mapear).ToList();
    }

    public async Task<ProductoResponse?> ObtenerAsync(int id)
    {
        var producto = await _db.Productos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        return producto is null ? null : Mapear(producto);
    }

    public async Task<ProductoResponse> CrearAsync(ProductoRequest request)
    {
        var producto = new Producto
        {
            Nombre = request.Nombre.Trim(),
            PrecioUnitario = request.PrecioUnitario
        };

        _db.Productos.Add(producto);
        await _db.SaveChangesAsync();
        return Mapear(producto);
    }

    public async Task<ProductoResponse?> ActualizarAsync(int id, ProductoRequest request)
    {
        var producto = await _db.Productos.FindAsync(id);
        if (producto is null) return null;

        producto.Nombre = request.Nombre.Trim();
        producto.PrecioUnitario = request.PrecioUnitario;

        await _db.SaveChangesAsync();
        return Mapear(producto);
    }

    // Borrado lógico: el producto puede estar en facturas emitidas y debe conservarse.
    public async Task<bool> DesactivarAsync(int id)
    {
        var producto = await _db.Productos.FindAsync(id);
        if (producto is null) return false;

        producto.Activo = false;
        await _db.SaveChangesAsync();
        return true;
    }

    private static ProductoResponse Mapear(Producto p) =>
        new(p.Id, p.Nombre, p.PrecioUnitario, p.Activo);
}