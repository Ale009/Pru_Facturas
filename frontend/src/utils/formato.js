const moneda = new Intl.NumberFormat("es-GT", { style: "currency", currency: "GTQ" });

export const formatoMoneda = (valor) => moneda.format(valor);

// La API entrega fechas en UTC pero sin la "Z" final; sin ella el navegador
// las interpretaría como hora local y mostraría una hora incorrecta.
export function formatoFecha(texto) {
  if (!texto) return "";
  const utc = /(Z|[+-]\d{2}:\d{2})$/.test(texto) ? texto : `${texto}Z`;
  return new Date(utc).toLocaleString("es-GT");
}
