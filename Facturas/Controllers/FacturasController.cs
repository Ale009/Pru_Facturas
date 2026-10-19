using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Facturas.Dtos;
using Facturas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Facturas.Models;

namespace Facturas.Controllers;

/// <summary>Emisión y consulta de facturas. Requiere un token JWT válido.</summary>
[ApiController]
[Authorize]
[Route("api/[controller]")]
public class FacturasController : ControllerBase
{
    private readonly IFacturaService _facturas;

    public FacturasController(IFacturaService facturas) => _facturas = facturas;

    /// <summary>Reporte de facturas. Filtro opcional estado=Emitida o estado=Anulada.</summary>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<FacturaResumenResponse>>> Listar([FromQuery] EstadoFactura? estado)
        => Ok(await _facturas.ListarAsync(estado));
    
    [HttpPost]
    public async Task<ActionResult<FacturaResponse>> Crear(FacturaCrearRequest request)
    {
        var usuarioId = ObtenerUsuarioId();
        if (usuarioId is null) return Unauthorized();

        var creada = await _facturas.CrearAsync(request, usuarioId.Value);
        return CreatedAtAction(nameof(Obtener), new { id = creada.Id }, creada);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<FacturaResponse>> Obtener(int id)
    {
        var factura = await _facturas.ObtenerAsync(id);
        return factura is null ? NotFound() : Ok(factura);
    }

    // El usuario sale del token (claim "sub"), nunca de lo que envíe el cliente.
    private int? ObtenerUsuarioId()
    {
        var valor = User.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        return int.TryParse(valor, out var id) ? id : null;
    }
    
    /// <summary>Anula una factura. No la elimina: conserva el registro con su motivo.</summary>
    [HttpPost("{id:int}/anular")]
    public async Task<ActionResult<FacturaResponse>> Anular(int id, AnularFacturaRequest request)
    {
        var anulada = await _facturas.AnularAsync(id, request.Motivo);
        return anulada is null ? NotFound() : Ok(anulada);
    }
}
