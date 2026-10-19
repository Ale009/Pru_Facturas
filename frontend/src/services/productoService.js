import { api } from "../api/client";

export const productoService = {
  listar: (soloActivos = false) => api.get(`/productos?soloActivos=${soloActivos}`),
  crear: (datos) => api.post("/productos", datos),
  actualizar: (id, datos) => api.put(`/productos/${id}`, datos),
  desactivar: (id) => api.delete(`/productos/${id}`),
};
