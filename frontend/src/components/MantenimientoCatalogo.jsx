import { useCallback, useEffect, useState } from "react";

const claseInput =
  "w-full rounded-lg border border-slate-300 px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500";

/**
 * Mantenimiento genérico de un catálogo (listar, crear, editar, desactivar).
 * Se configura con:
 *  - servicio: objeto con listar/crear/actualizar/desactivar
 *  - campos:   campos del formulario [{ nombre, etiqueta, tipo, requerido, maxLength }]
 *  - columnas: columnas de la tabla  [{ clave, etiqueta, formato? }]
 */
export default function MantenimientoCatalogo({ servicio, campos, columnas }) {
  const vacio = () => Object.fromEntries(campos.map((c) => [c.nombre, ""]));

  const [filas, setFilas] = useState([]);
  const [formulario, setFormulario] = useState(vacio);
  const [editandoId, setEditandoId] = useState(null);
  const [error, setError] = useState("");
  const [cargando, setCargando] = useState(true);
  const [guardando, setGuardando] = useState(false);

  const cargar = useCallback(async () => {
    try {
      setFilas(await servicio.listar());
    } catch (e) {
      setError(e.message);
    } finally {
      setCargando(false);
    }
  }, [servicio]);

  useEffect(() => {
    cargar();
  }, [cargar]);

  const cancelar = () => {
    setEditandoId(null);
    setFormulario(vacio());
  };

  const editar = (fila) => {
    setEditandoId(fila.id);
    setFormulario(Object.fromEntries(campos.map((c) => [c.nombre, fila[c.nombre] ?? ""])));
  };

  const guardar = async (e) => {
    e.preventDefault();
    setError("");
    setGuardando(true);

    const datos = Object.fromEntries(
      campos.map((c) => [c.nombre, c.tipo === "number" ? Number(formulario[c.nombre]) : formulario[c.nombre]])
    );

    try {
      if (editandoId) await servicio.actualizar(editandoId, datos);
      else await servicio.crear(datos);
      cancelar();
      await cargar();
    } catch (err) {
      setError(err.message);
    } finally {
      setGuardando(false);
    }
  };

  const desactivar = async (fila) => {
    if (!window.confirm("¿Desactivar este registro?")) return;
    setError("");
    try {
      await servicio.desactivar(fila.id);
      await cargar();
    } catch (err) {
      setError(err.message);
    }
  };

  return (
    <div className="space-y-6">
      <form onSubmit={guardar} className="bg-white rounded-xl shadow-sm p-4 space-y-4">
        <h2 className="font-semibold text-slate-800">{editandoId ? "Editar registro" : "Nuevo registro"}</h2>

        <div className="grid gap-4 sm:grid-cols-3">
          {campos.map((c) => (
            <div key={c.nombre}>
              <label htmlFor={c.nombre} className="block text-sm font-medium text-slate-700 mb-1">
                {c.etiqueta}
              </label>
              <input
                id={c.nombre}
                type={c.tipo ?? "text"}
                step={c.tipo === "number" ? "0.01" : undefined}
                min={c.tipo === "number" ? "0.01" : undefined}
                maxLength={c.maxLength}
                required={c.requerido}
                value={formulario[c.nombre]}
                onChange={(e) => setFormulario({ ...formulario, [c.nombre]: e.target.value })}
                className={claseInput}
              />
            </div>
          ))}
        </div>

        {error && (
          <p role="alert" className="text-sm text-red-600 bg-red-50 rounded-lg px-3 py-2">
            {error}
          </p>
        )}

        <div className="flex gap-2">
          <button
            type="submit"
            disabled={guardando}
            className="bg-blue-600 hover:bg-blue-700 disabled:bg-blue-300 text-white font-medium rounded-lg px-4 py-2"
          >
            {guardando ? "Guardando..." : "Guardar"}
          </button>
          {editandoId && (
            <button
              type="button"
              onClick={cancelar}
              className="bg-slate-200 hover:bg-slate-300 text-slate-800 font-medium rounded-lg px-4 py-2"
            >
              Cancelar
            </button>
          )}
        </div>
      </form>

      <div className="bg-white rounded-xl shadow-sm overflow-x-auto">
        <table className="w-full text-sm">
          <thead className="bg-slate-50 text-left text-slate-600">
            <tr>
              {columnas.map((c) => (
                <th key={c.clave} className="px-4 py-3 font-medium">
                  {c.etiqueta}
                </th>
              ))}
              <th className="px-4 py-3" />
            </tr>
          </thead>
          <tbody>
            {cargando && (
              <tr>
                <td className="px-4 py-3 text-slate-500" colSpan={columnas.length + 1}>
                  Cargando...
                </td>
              </tr>
            )}
            {!cargando && filas.length === 0 && (
              <tr>
                <td className="px-4 py-3 text-slate-500" colSpan={columnas.length + 1}>
                  Sin registros.
                </td>
              </tr>
            )}
            {filas.map((fila) => (
              <tr key={fila.id} className="border-t border-slate-100">
                {columnas.map((c) => (
                  <td key={c.clave} className="px-4 py-3">
                    {c.formato ? c.formato(fila[c.clave]) : fila[c.clave]}
                  </td>
                ))}
                <td className="px-4 py-3 text-right space-x-2 whitespace-nowrap">
                  <button onClick={() => editar(fila)} className="text-blue-600 hover:underline">
                    Editar
                  </button>
                  <button
                    onClick={() => desactivar(fila)}
                    disabled={!fila.activo}
                    className="text-red-600 hover:underline disabled:text-slate-400 disabled:no-underline"
                  >
                    Desactivar
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}
