namespace Facturas.Dtos;

/// <summary>Representación de un producto que devuelve la API.</summary>
public record ProductoResponse(int Id, string Nombre, decimal PrecioUnitario, bool Activo);