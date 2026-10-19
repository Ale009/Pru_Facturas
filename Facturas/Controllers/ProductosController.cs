using Facturas.Dtos;
using Facturas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Facturas.Controllers;

/// <summary>Mantenimiento de productos. Requiere un token JWT válido.</summary>
[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly IProductoService _productos;

    public ProductosController(IProductoService productos) => _productos = productos;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductoResponse>>> Listar([FromQuery] bool soloActivos = false)
        => Ok(await _productos.ListarAsync(soloActivos));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductoResponse>> Obtener(int id)
    {
        var producto = await _productos.ObtenerAsync(id);
        return producto is null ? NotFound() : Ok(producto);
    }

    [HttpPost]
    public async Task<ActionResult<ProductoResponse>> Crear(ProductoRequest request)
    {
        var creado = await _productos.CrearAsync(request);
        return CreatedAtAction(nameof(Obtener), new { id = creado.Id }, creado);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProductoResponse>> Actualizar(int id, ProductoRequest request)
    {
        var actualizado = await _productos.ActualizarAsync(id, request);
        return actualizado is null ? NotFound() : Ok(actualizado);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Desactivar(int id)
        => await _productos.DesactivarAsync(id) ? NoContent() : NotFound();
}