import { createContext, useCallback, useContext, useEffect, useState } from "react";
import { login as loginApi } from "../services/authService";
import { guardarSesion, limpiarSesion, obtenerSesion } from "./session";

const AuthContext = createContext(null);

export function AuthProvider({ children }) {
  const [sesion, setSesion] = useState(() => obtenerSesion());

  const iniciarSesion = useCallback(async (usuario, password) => {
    const datos = await loginApi(usuario, password);
    guardarSesion(datos);
    setSesion(datos);
  }, []);

  const cerrarSesion = useCallback(() => {
    limpiarSesion();
    setSesion(null);
  }, []);

  // El cliente HTTP avisa cuando la API responde 401 con una sesión activa.
  useEffect(() => {
    window.addEventListener("sesion-expirada", cerrarSesion);
    return () => window.removeEventListener("sesion-expirada", cerrarSesion);
  }, [cerrarSesion]);

  return (
    <AuthContext.Provider value={{ sesion, iniciarSesion, cerrarSesion }}>
      {children}
    </AuthContext.Provider>
  );
}

export const useAuth = () => useContext(AuthContext);
