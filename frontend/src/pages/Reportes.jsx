import { useEffect, useMemo, useState } from "react";
import { facturaService } from "../services/facturaService";
import { formatoFecha, formatoMoneda } from "../utils/formato";

const filtros = [
  { valor: "", etiqueta: "Todas" },
  { valor: "Emitida", etiqueta: "Emitidas" },
  { valor: "Anulada", etiqueta: "Anuladas" },
];

const estilosEstado = {
  Emitida: "bg-green-100 text-green-800",
  Anulada: "bg-red-100 text-red-800",
};

export default function Reportes() {
  const [estado, setEstado] = useState("");
  const [facturas, setFacturas] = useState([]);
  const [error, setError] = useState("");
  const [cargando, setCargando] = useState(true);

  useEffect(() => {
    // Si el usuario cambia de filtro antes de que responda la petición anterior,
    // se ignora la respuesta vieja para que no pise a la nueva.
    let cancelado = false;
    setCargando(true);
    setError("");

    facturaService
      .listar(estado)
      .then((datos) => {
        if (!cancelado) setFacturas(datos);
      })
      .catch((e) => {
        if (!cancelado) setError(e.message);
      })
      .finally(() => {
        if (!cancelado) setCargando(false);
      });

    return () => {
      cancelado = true;
    };
  }, [estado]);

  // Las facturas anuladas no suman al valor emitido.
  const totalEmitido = useMemo(
    () => facturas.filter((f) => f.estado === "Emitida").reduce((suma, f) => suma + f.total, 0),
    [facturas]
  );

  return (
    <div className="space-y-4">
      <h1 className="text-xl font-semibold text-slate-800">Reportes</h1>

      <div className="flex flex-wrap items-center gap-2">
        {filtros.map((f) => (
          <button
            key={f.valor}
            onClick={() => setEstado(f.valor)}
            className={`px-4 py-2 rounded-lg text-sm font-medium ${
              estado === f.valor ? "bg-blue-600 text-white" : "bg-white text-slate-700 hover:bg-slate-200"
            }`}
          >
            {f.etiqueta}
          </button>
        ))}
      </div>

      {error && (
        <p role="alert" className="text-sm text-red-600 bg-red-50 rounded-lg px-3 py-2">
          {error}
        </p>
      )}

      <div className="bg-white rounded-xl shadow-sm overflow-x-auto">
        <table className="w-full text-sm">
          <thead className="bg-slate-50 text-left text-slate-600">
            <tr>
              <th className="px-4 py-3 font-medium">No.</th>
              <th className="px-4 py-3 font-medium">Fecha</th>
              <th className="px-4 py-3 font-medium">Cliente</th>
              <th className="px-4 py-3 font-medium text-right">Valor</th>
              <th className="px-4 py-3 font-medium">Estado</th>
            </tr>
          </thead>
          <tbody>
            {cargando && (
              <tr>
                <td className="px-4 py-3 text-slate-500" colSpan={5}>
                  Cargando...
                </td>
              </tr>
            )}
            {!cargando && facturas.length === 0 && (
              <tr>
                <td className="px-4 py-3 text-slate-500" colSpan={5}>
                  No hay facturas para mostrar.
                </td>
              </tr>
            )}
            {facturas.map((f) => (
              <tr key={f.id} className="border-t border-slate-100">
                <td className="px-4 py-3">{f.id}</td>
                <td className="px-4 py-3">{formatoFecha(f.fecha)}</td>
                <td className="px-4 py-3">{f.cliente}</td>
                <td className="px-4 py-3 text-right">{formatoMoneda(f.total)}</td>
                <td className="px-4 py-3">
                  <span
                    className={`px-2 py-1 rounded-full text-xs font-medium ${
                      estilosEstado[f.estado] ?? "bg-slate-100 text-slate-700"
                    }`}
                  >
                    {f.estado}
                  </span>
                </td>
              </tr>
            ))}
          </tbody>
          {!cargando && facturas.length > 0 && (
            <tfoot>
              <tr className="border-t border-slate-200 font-semibold">
                <td className="px-4 py-3" colSpan={3}>
                  {facturas.length} factura(s). Valor emitido (sin anuladas)
                </td>
                <td className="px-4 py-3 text-right">{formatoMoneda(totalEmitido)}</td>
                <td />
              </tr>
            </tfoot>
          )}
        </table>
      </div>
    </div>
  );
}
