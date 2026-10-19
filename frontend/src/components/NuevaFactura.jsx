import { useEffect, useMemo, useState } from "react";
import { clienteService } from "../services/clienteService";
import { facturaService } from "../services/facturaService";
import { productoService } from "../services/productoService";
import { formatoMoneda } from "../utils/formato";

const claseInput =
  "w-full rounded-lg border border-slate-300 px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500";

/** Formulario maestro-detalle: un cliente y varias líneas de producto. */
export default function NuevaFactura() {
  const [clientes, setClientes] = useState([]);
  const [productos, setProductos] = useState([]);
  const [clienteId, setClienteId] = useState("");
  const [productoId, setProductoId] = useState("");
  const [cantidad, setCantidad] = useState(1);
  const [lineas, setLineas] = useState([]);
  const [error, setError] = useState("");
  const [exito, setExito] = useState("");
  const [guardando, setGuardando] = useState(false);

  // Solo se ofrecen clientes y productos activos.
  useEffect(() => {
    Promise.all([clienteService.listar(true), productoService.listar(true)])
      .then(([c, p]) => {
        setClientes(c);
        setProductos(p);
      })
      .catch((e) => setError(e.message));
  }, []);

  // Total de vista previa. El valor oficial lo calcula la API al emitir.
  const total = useMemo(
    () => lineas.reduce((suma, l) => suma + l.precioUnitario * l.cantidad, 0),
    [lineas]
  );

  const agregarLinea = () => {
    const producto = productos.find((p) => p.id === Number(productoId));
    if (!producto || cantidad < 1) return;

    // Estado inmutable: siempre se crea un arreglo nuevo, nunca se modifica el anterior.
    setLineas((previas) => {
      const existente = previas.find((l) => l.productoId === producto.id);
      if (existente) {
        return previas.map((l) =>
          l.productoId === producto.id ? { ...l, cantidad: l.cantidad + cantidad } : l
        );
      }
      return [
        ...previas,
        {
          productoId: producto.id,
          nombre: producto.nombre,
          precioUnitario: producto.precioUnitario,
          cantidad,
        },
      ];
    });
    setProductoId("");
    setCantidad(1);
  };

  const quitarLinea = (id) => setLineas((previas) => previas.filter((l) => l.productoId !== id));

  const emitir = async () => {
    setError("");
    setExito("");
    setGuardando(true);
    try {
      // Solo se envían ids y cantidades: los precios los toma el servidor del catálogo.
      const factura = await facturaService.crear({
        clienteId: Number(clienteId),
        detalles: lineas.map((l) => ({ productoId: l.productoId, cantidad: l.cantidad })),
      });
      setExito(`Factura #${factura.id} emitida por ${formatoMoneda(factura.total)}.`);
      setLineas([]);
      setClienteId("");
    } catch (e) {
      setError(e.message);
    } finally {
      setGuardando(false);
    }
  };

  const puedeEmitir = clienteId && lineas.length > 0 && !guardando;

  return (
    <div className="space-y-6">
      <div className="bg-white rounded-xl shadow-sm p-4 space-y-4">
        <div>
          <label htmlFor="cliente" className="block text-sm font-medium text-slate-700 mb-1">
            Cliente
          </label>
          <select
            id="cliente"
            value={clienteId}
            onChange={(e) => setClienteId(e.target.value)}
            className={claseInput}
          >
            <option value="">Seleccione un cliente...</option>
            {clientes.map((c) => (
              <option key={c.id} value={c.id}>
                {c.nombre} ({c.nit})
              </option>
            ))}
          </select>
        </div>

        <div className="grid gap-4 sm:grid-cols-[1fr_8rem_auto] items-end">
          <div>
            <label htmlFor="producto" className="block text-sm font-medium text-slate-700 mb-1">
              Producto
            </label>
            <select
              id="producto"
              value={productoId}
              onChange={(e) => setProductoId(e.target.value)}
              className={claseInput}
            >
              <option value="">Seleccione un producto...</option>
              {productos.map((p) => (
                <option key={p.id} value={p.id}>
                  {p.nombre} - {formatoMoneda(p.precioUnitario)}
                </option>
              ))}
            </select>
          </div>

          <div>
            <label htmlFor="cantidad" className="block text-sm font-medium text-slate-700 mb-1">
              Cantidad
            </label>
            <input
              id="cantidad"
              type="number"
              min="1"
              max="10000"
              value={cantidad}
              onChange={(e) => setCantidad(Number(e.target.value))}
              className={claseInput}
            />
          </div>

          <button
            type="button"
            onClick={agregarLinea}
            disabled={!productoId || cantidad < 1}
            className="bg-slate-700 hover:bg-slate-800 disabled:bg-slate-300 text-white font-medium rounded-lg px-4 py-2"
          >
            Agregar
          </button>
        </div>
      </div>

      <div className="bg-white rounded-xl shadow-sm overflow-x-auto">
        <table className="w-full text-sm">
          <thead className="bg-slate-50 text-left text-slate-600">
            <tr>
              <th className="px-4 py-3 font-medium">Producto</th>
              <th className="px-4 py-3 font-medium text-right">Cantidad</th>
              <th className="px-4 py-3 font-medium text-right">Precio</th>
              <th className="px-4 py-3 font-medium text-right">Subtotal</th>
              <th className="px-4 py-3" />
            </tr>
          </thead>
          <tbody>
            {lineas.length === 0 && (
              <tr>
                <td className="px-4 py-3 text-slate-500" colSpan={5}>
                  Agregue al menos un producto.
                </td>
              </tr>
            )}
            {lineas.map((l) => (
              <tr key={l.productoId} className="border-t border-slate-100">
                <td className="px-4 py-3">{l.nombre}</td>
                <td className="px-4 py-3 text-right">{l.cantidad}</td>
                <td className="px-4 py-3 text-right">{formatoMoneda(l.precioUnitario)}</td>
                <td className="px-4 py-3 text-right">{formatoMoneda(l.precioUnitario * l.cantidad)}</td>
                <td className="px-4 py-3 text-right">
                  <button onClick={() => quitarLinea(l.productoId)} className="text-red-600 hover:underline">
                    Quitar
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
          <tfoot>
            <tr className="border-t border-slate-200 font-semibold">
              <td className="px-4 py-3" colSpan={3}>
                Total
              </td>
              <td className="px-4 py-3 text-right">{formatoMoneda(total)}</td>
              <td />
            </tr>
          </tfoot>
        </table>
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
        onClick={emitir}
        disabled={!puedeEmitir}
        className="bg-blue-600 hover:bg-blue-700 disabled:bg-blue-300 text-white font-semibold rounded-lg px-5 py-2"
      >
        {guardando ? "Emitiendo..." : "Emitir factura"}
      </button>
    </div>
  );
}
