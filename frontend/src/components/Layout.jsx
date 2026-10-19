import { NavLink, Outlet } from "react-router-dom";
import { useAuth } from "../auth/AuthContext";

const enlaces = [
  { to: "/ingreso", texto: "Ingreso" },
  { to: "/reportes", texto: "Reportes" },
  { to: "/mantenimientos", texto: "Mantenimientos" },
];

const claseEnlace = ({ isActive }) =>
  `px-3 py-2 rounded-lg text-sm font-medium ${
    isActive ? "bg-blue-600 text-white" : "text-slate-700 hover:bg-slate-200"
  }`;

export default function Layout() {
  const { sesion, cerrarSesion } = useAuth();

  return (
    <div className="min-h-screen bg-slate-100">
      <header className="bg-white shadow-sm">
        <div className="max-w-6xl mx-auto px-4 py-3 flex items-center justify-between">
          <nav className="flex items-center gap-2">
            <span className="font-bold text-slate-800 mr-4">Sistema de Facturas</span>
            {enlaces.map((e) => (
              <NavLink key={e.to} to={e.to} className={claseEnlace}>
                {e.texto}
              </NavLink>
            ))}
          </nav>

          <div className="flex items-center gap-3 text-sm">
            <span className="text-slate-600">{sesion.usuario}</span>
            <button
              onClick={cerrarSesion}
              className="px-3 py-2 rounded-lg bg-slate-700 hover:bg-slate-800 text-white font-medium"
            >
              Salida
            </button>
          </div>
        </div>
      </header>

      <main className="max-w-6xl mx-auto px-4 py-6">
        <Outlet />
      </main>
    </div>
  );
}
