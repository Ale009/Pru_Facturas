namespace Facturas.Dtos;

/// <summary>Fila del reporte de facturas: encabezado resumido, sin las líneas.</summary>
public record FacturaResumenResponse(int Id, DateTime Fecha, string Cliente, decimal Total, string Estado);