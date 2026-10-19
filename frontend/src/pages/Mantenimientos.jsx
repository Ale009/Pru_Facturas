import { useState } from "react";
import MantenimientoCatalogo from "../components/MantenimientoCatalogo";
import { clienteService } from "../services/clienteService";
import { productoService } from "../services/productoService";
import { formatoMoneda } from "../utils/formato";

const siNo = (valor) => (valor ? "Sí" : "No");

// Cada catálogo se describe con datos; el componente genérico hace el trabajo.
const catalogos = {
  clientes: {
    etiqueta: "Clientes",
    servicio: clienteService,
    campos: [
      { nombre: "nombre", etiqueta: "Nombre", requerido: true, maxLength: 100 },
      { nombre: "nit", etiqueta: "NIT", requerido: true, maxLength: 20 },
      { nombre: "direccion", etiqueta: "Dirección", maxLength: 200 },
    ],
    columnas: [
      { clave: "nombre", etiqueta: "Nombre" },
      { clave: "nit", etiqueta: "NIT" },
      { clave: "direccion", etiqueta: "Dirección" },
      { clave: "activo", etiqueta: "Activo", formato: siNo },
    ],
  },
  productos: {
    etiqueta: "Productos",
    servicio: productoService,
    campos: [
      { nombre: "nombre", etiqueta: "Nombre", requerido: true, maxLength: 100 },
      { nombre: "precioUnitario", etiqueta: "Precio unitario", tipo: "number", requerido: true },
    ],
    columnas: [
      { clave: "nombre", etiqueta: "Nombre" },
      { clave: "precioUnitario", etiqueta: "Precio", formato: formatoMoneda },
      { clave: "activo", etiqueta: "Activo", formato: siNo },
    ],
  },
};

export default function Mantenimientos() {
  const [actual, setActual] = useState("clientes");
  const catalogo = catalogos[actual];

  return (
    <div className="space-y-4">
      <h1 className="text-xl font-semibold text-slate-800">Mantenimientos</h1>

      <div className="flex gap-2">
        {Object.entries(catalogos).map(([clave, c]) => (
          <button
            key={clave}
            onClick={() => setActual(clave)}
            className={`px-4 py-2 rounded-lg text-sm font-medium ${
              actual === clave ? "bg-blue-600 text-white" : "bg-white text-slate-700 hover:bg-slate-200"
            }`}
          >
            {c.etiqueta}
          </button>
        ))}
      </div>

      {/* key fuerza a reiniciar el estado del componente al cambiar de catálogo */}
      <MantenimientoCatalogo
        key={actual}
        servicio={catalogo.servicio}
        campos={catalogo.campos}
        columnas={catalogo.columnas}
      />
    </div>
  );
}
