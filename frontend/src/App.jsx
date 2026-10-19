import { Navigate, Route, Routes } from "react-router-dom";
import Layout from "./components/Layout";
import RutaProtegida from "./components/RutaProtegida";
import Ingreso from "./pages/Ingreso";
import Login from "./pages/Login";
import Mantenimientos from "./pages/Mantenimientos";
import Reportes from "./pages/Reportes";

export default function App() {
  return (
    <Routes>
      <Route path="/login" element={<Login />} />

      <Route element={<RutaProtegida />}>
        <Route element={<Layout />}>
          <Route path="/" element={<Navigate to="/ingreso" replace />} />
          <Route path="/ingreso" element={<Ingreso />} />
          <Route path="/reportes" element={<Reportes />} />
          <Route path="/mantenimientos" element={<Mantenimientos />} />
        </Route>
      </Route>

      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  );
}
