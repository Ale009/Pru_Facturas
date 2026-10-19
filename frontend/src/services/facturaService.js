import { api } from "../api/client";

export const facturaService = {
  listar: (estado) => api.get(estado ? `/facturas?estado=${estado}` : "/facturas"),
  obtener: (id) => api.get(`/facturas/${id}`),
  crear: (datos) => api.post("/facturas", datos),
  anular: (id, motivo) => api.post(`/facturas/${id}/anular`, { motivo }),
};
