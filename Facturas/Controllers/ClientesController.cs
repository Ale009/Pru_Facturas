using Facturas.Dtos;
using Facturas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Facturas.Controllers;

/// <summary>Mantenimiento de clientes. Requiere un token JWT válido.</summary>
[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _clientes;

    public ClientesController(IClienteService clientes) => _clientes = clientes;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ClienteResponse>>> Listar([FromQuery] bool soloActivos = false)
        => Ok(await _clientes.ListarAsync(soloActivos));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ClienteResponse>> Obtener(int id)
    {
        var cliente = await _clientes.ObtenerAsync(id);
        return cliente is null ? NotFound() : Ok(cliente);
    }

    [HttpPost]
    public async Task<ActionResult<ClienteResponse>> Crear(ClienteRequest request)
    {
        var creado = await _clientes.CrearAsync(request);
        return CreatedAtAction(nameof(Obtener), new { id = creado.Id }, creado);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ClienteResponse>> Actualizar(int id, ClienteRequest request)
    {
        var actualizado = await _clientes.ActualizarAsync(id, request);
        return actualizado is null ? NotFound() : Ok(actualizado);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Desactivar(int id)
        => await _clientes.DesactivarAsync(id) ? NoContent() : NotFound();
}