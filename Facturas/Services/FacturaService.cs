using Facturas.Data;
using Facturas.Dtos;
using Facturas.Exceptions;
using Facturas.Models;
using Microsoft.EntityFrameworkCore;

namespace Facturas.Services;

/// <summary>Emite y consulta facturas (maestro-detalle).</summary>
public class FacturaService : IFacturaService
{
    private readonly AppDbContext _db;

    public FacturaService(AppDbContext db) => _db = db;

    public async Task<FacturaResponse> CrearAsync(FacturaCrearRequest request, int usuarioId)
    {
        var cliente = await _db.Clientes.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == request.ClienteId && c.Activo)
            ?? throw new ReglaNegocioException("El cliente no existe o está inactivo.");

        // Si el mismo producto llega en varias líneas, se consolidan en una sola.
        var lineas = request.Detalles
            .GroupBy(d => d.ProductoId)
            .Select(g => new { ProductoId = g.Key, Cantidad = g.Sum(x => x.Cantidad) })
            .ToList();

        var idsProductos = lineas.Select(l => l.ProductoId).ToList();

        // Una sola consulta para todos los productos de la factura.
        var productos = await _db.Productos
            .Where(p => idsProductos.Contains(p.Id) && p.Activo)
            .ToDictionaryAsync(p => p.Id);

        if (productos.Count != idsProductos.Count)
            throw new ReglaNegocioException("Uno o más productos no existen o están inactivos.");

        var factura = new Factura
        {
            ClienteId = cliente.Id,
            UsuarioId = usuarioId,
            Fecha = DateTime.UtcNow,
            Estado = EstadoFactura.Emitida
        };

        foreach (var linea in lineas)
        {
            var producto = productos[linea.ProductoId];

            // El precio se copia del catálogo en este momento: el cliente de la API no lo controla.
            factura.Detalles.Add(new FacturaDetalle
            {
                ProductoId = producto.Id,
                Cantidad = linea.Cantidad,
                PrecioUnitario = producto.PrecioUnitario,
                Subtotal = producto.PrecioUnitario * linea.Cantidad
            });
        }

        factura.Total = factura.Detalles.Sum(d => d.Subtotal);

        // SaveChanges guarda encabezado y líneas en una sola transacción: o todo, o nada.
        _db.Facturas.Add(factura);
        await _db.SaveChangesAsync();

        return (await ObtenerAsync(factura.Id))!;
    }

    public async Task<FacturaResponse?> ObtenerAsync(int id)
    {
        var factura = await _db.Facturas
            .AsNoTracking()
            .Include(f => f.Cliente)
            .Include(f => f.Usuario)
            .Include(f => f.Detalles).ThenInclude(d => d.Producto)
            .FirstOrDefaultAsync(f => f.Id == id);

        return factura is null ? null : Mapear(factura);
    }

    public async Task<FacturaResponse?> AnularAsync(int id, string motivo)
    {
        // UPDATE condicional y atómico: solo anula si la factura sigue "Emitida".
        // Así, dos peticiones simultáneas no pueden anularla dos veces.
        var filas = await _db.Facturas
            .Where(f => f.Id == id && f.Estado == EstadoFactura.Emitida)
            .ExecuteUpdateAsync(s => s
                .SetProperty(f => f.Estado, EstadoFactura.Anulada)
                .SetProperty(f => f.FechaAnulacion, (DateTime?)DateTime.UtcNow)
                .SetProperty(f => f.MotivoAnulacion, motivo.Trim()));

        if (filas == 1)
            return await ObtenerAsync(id);

        // No se actualizó ninguna fila: o no existe, o ya estaba anulada.
        var existe = await _db.Facturas.AnyAsync(f => f.Id == id);
        if (!existe) return null;

        throw new ReglaNegocioException("La factura ya está anulada.");
    }
    public async Task<IReadOnlyList<FacturaResumenResponse>> ListarAsync(EstadoFactura? estado)
    {
        var consulta = _db.Facturas.AsNoTracking();

        // El filtro se compone sobre la consulta y se ejecuta en MySQL, no en memoria.
        if (estado is not null)
            consulta = consulta.Where(f => f.Estado == estado);

        // Proyección: solo se leen las columnas necesarias, sin cargar las líneas.
        var filas = await consulta
            .OrderByDescending(f => f.Fecha)
            .Select(f => new { f.Id, f.Fecha, Cliente = f.Cliente.Nombre, f.Total, f.Estado })
            .ToListAsync();

        return filas
            .Select(f => new FacturaResumenResponse(f.Id, f.Fecha, f.Cliente, f.Total, f.Estado.ToString()))
            .ToList();
    }
    private static FacturaResponse Mapear(Factura f) => new(
        f.Id,
        f.Fecha,
        f.ClienteId,
        f.Cliente.Nombre,
        f.Usuario.NombreUsuario,
        f.Total,
        f.Estado.ToString(),
        f.FechaAnulacion,
        f.MotivoAnulacion,
        f.Detalles
            .Select(d => new FacturaDetalleResponse(
                d.ProductoId, d.Producto.Nombre, d.Cantidad, d.PrecioUnitario, d.Subtotal))
            .ToList());
}