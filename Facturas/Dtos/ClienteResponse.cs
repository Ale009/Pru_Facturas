namespace Facturas.Dtos;

/// <summary>Representación de un cliente que devuelve la API.</summary>
public record ClienteResponse(int Id, string Nombre, string Nit, string? Direccion, bool Activo);