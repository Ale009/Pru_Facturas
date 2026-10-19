import { useCallback, useEffect, useState } from "react";
import { facturaService } from "../services/facturaService";
import { formatoMoneda } from "../utils/formato";

const claseInput =
  "w-full rounded-lg border border-slate-300 px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500";

/** Anula una factura emitida. La factura no se elimina: queda con estado y motivo. */
export default function AnularFactura() {
  const [facturas, setFacturas] = useState([]);
  const [id, setId] = useState("");
  const [motivo, setMotivo] = useState("");
  const [error, setError] = useState("");
  const [exito, setExito] = useState("");
  const [guardando, setGuardando] = useState(false);

  // Solo las facturas en estado Emitida pueden anularse.
  const cargar = useCallback(async () => {
    try {
      setFacturas(await facturaService.listar("Emitida"));
    } catch (e) {
      setError(e.message);
    }
  }, []);

  useEffect(() => {
    cargar();
  }, [cargar]);

  const anular = async (e) => {
    e.preventDefault();
    if (!window.confirm(`¿Anular la factura #${id}? Esta acción no se puede deshacer.`)) return;

    setError("");
    setExito("");
    setGuardando(true);
    try {
      await facturaService.anular(Number(id), motivo.trim());
      setExito(`Factura #${id} anulada.`);
      setId("");
      setMotivo("");
      await cargar();
    } catch (err) {
      setError(err.message);
    } finally {
      setGuardando(false);
    }
  };

  return (
    <form onSubmit={anular} className="bg-white rounded-xl shadow-sm p-4 space-y-4 max-w-xl">
      <div>
        <label htmlFor="factura" className="block text-sm font-medium text-slate-700 mb-1">
          Factura emitida
        </label>
        <select
          id="factura"
          value={id}
          onChange={(e) => setId(e.target.value)}
          required
          className={claseInput}
        >
          <option value="">Seleccione una factura...</option>
          {facturas.map((f) => (
            <option key={f.id} value={f.id}>
              #{f.id} - {f.cliente} - {formatoMoneda(f.total)}
            </option>
          ))}
        </select>
        {facturas.length === 0 && <p className="text-sm text-slate-500 mt-1">No hay facturas emitidas.</p>}
      </div>

      <div>
        <label htmlFor="motivo" className="block text-sm font-medium text-slate-700 mb-1">
          Motivo de la anulación
        </label>
        <textarea
          id="motivo"
          value={motivo}
          onChange={(e) => setMotivo(e.target.value)}
          required
          minLength={5}
          maxLength={200}
          rows={3}
          className={claseInput}
        />
      </div>

      {error && (
        <p role="alert" className="text-sm text-red-600 bg-red-50 rounded-lg px-3 py-2">
          {error}
        </p>
      )}
      {exito && (
        <p role="status" className="text-sm text-green-700 bg-green-50 rounded-lg px-3 py-2">
          {exito}
        </p>
      )}

      <button
        type="submit"
        disabled={guardando || !id}
        className="bg-red-600 hover:bg-red-700 disabled:bg-red-300 text-white font-semibold rounded-lg px-5 py-2"
      >
        {guardando ? "Anulando..." : "Anular factura"}
      </button>
    </form>
  );
}
