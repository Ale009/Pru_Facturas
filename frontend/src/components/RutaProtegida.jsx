import { Navigate, Outlet } from "react-router-dom";
import { useAuth } from "../auth/AuthContext";

// Guarda de navegación (experiencia de usuario). La seguridad real la impone la API con [Authorize].
export default function RutaProtegida() {
  const { sesion } = useAuth();
  return sesion ? <Outlet /> : <Navigate to="/login" replace />;
}
