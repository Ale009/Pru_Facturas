const CLAVE = "sesion";

// La sesión vive en sessionStorage: se borra al cerrar la pestaña.
export function obtenerSesion() {
  try {
    const bruto = sessionStorage.getItem(CLAVE);
    if (!bruto) return null;

    const sesion = JSON.parse(bruto);

    // Un token vencido se descarta en el cliente; la API igualmente lo rechazaría.
    if (new Date(sesion.expiraEn) <= new Date()) {
      sessionStorage.removeItem(CLAVE);
      return null;
    }
    return sesion;
  } catch {
    return null;
  }
}

export function guardarSesion(sesion) {
  sessionStorage.setItem(CLAVE, JSON.stringify(sesion));
}

export function limpiarSesion() {
  sessionStorage.removeItem(CLAVE);
}
