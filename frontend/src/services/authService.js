import { api } from "../api/client";

export const login = (usuario, password) => api.post("/auth/login", { usuario, password });
