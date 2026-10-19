import { api } from "../api/client";

export const clienteService = {
  listar: (soloActivos = false) => api.get(`/clientes?soloActivos=${soloActivos}`),
  crear: (datos) => api.post("/clientes", datos),
  actualizar: (id, datos) => api.put(`/clientes/${id}`, datos),
  desactivar: (id) => api.delete(`/clientes/${id}`),
};
