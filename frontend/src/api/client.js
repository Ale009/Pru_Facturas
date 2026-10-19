import { limpiarSesion, obtenerSesion } from "../auth/session";

const API_URL = import.meta.env.VITE_API_URL;

export class ApiError extends Error {
  constructor(mensaje, status) {
    super(mensaje);
    this.status = status;
  }
}

// Convierte las respuestas de error de la API (ProblemDetails) en un mensaje legible.
function extraerMensaje(datos, status) {
  if (status === 429) return "Demasiados intentos. Espera un minuto.";
  if (datos?.errors) return Object.values(datos.errors).flat().join(" ");
  return datos?.detail || datos?.title || "Ocurrió un error inesperado.";
}

async function solicitar(metodo, ruta, cuerpo) {
  const sesion = obtenerSesion();
  const headers = { "Content-Type": "application/json" };
  if (sesion?.token) headers.Authorization = `Bearer ${sesion.token}`;

  let respuesta;
  try {
    respuesta = await fetch(`${API_URL}${ruta}`, {
      method: metodo,
      headers,
      body: cuerpo ? JSON.stringify(cuerpo) : undefined,
    });
  } catch {
    throw new ApiError("No se pudo conectar con el servidor.", 0);
  }

  // Token vencido o inválido: se cierra la sesión desde un único lugar.
  if (respuesta.status === 401 && sesion) {
    limpiarSesion();
    window.dispatchEvent(new Event("sesion-expirada"));
  }

  if (respuesta.status === 204) return null;

  const datos = await respuesta.json().catch(() => null);
  if (!respuesta.ok) throw new ApiError(extraerMensaje(datos, respuesta.status), respuesta.status);
  return datos;
}

export const api = {
  get: (ruta) => solicitar("GET", ruta),
  post: (ruta, cuerpo) => solicitar("POST", ruta, cuerpo),
  put: (ruta, cuerpo) => solicitar("PUT", ruta, cuerpo),
  delete: (ruta) => solicitar("DELETE", ruta),
};
