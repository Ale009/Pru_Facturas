import { useState } from "react";
import AnularFactura from "../components/AnularFactura";
import NuevaFactura from "../components/NuevaFactura";

const pestañas = [
  { clave: "agregar", etiqueta: "Agregar factura", Componente: NuevaFactura },
  { clave: "anular", etiqueta: "Anular factura", Componente: AnularFactura },
];

export default function Ingreso() {
  const [actual, setActual] = useState("agregar");
  const { Componente } = pestañas.find((p) => p.clave === actual);

  return (
    <div className="space-y-4">
      <h1 className="text-xl font-semibold text-slate-800">Ingreso</h1>

      <div className="flex gap-2">
        {pestañas.map((p) => (
          <button
            key={p.clave}
            onClick={() => setActual(p.clave)}
            className={`px-4 py-2 rounded-lg text-sm font-medium ${
              actual === p.clave ? "bg-blue-600 text-white" : "bg-white text-slate-700 hover:bg-slate-200"
            }`}
          >
            {p.etiqueta}
          </button>
        ))}
      </div>

      <Componente />
    </div>
  );
}
