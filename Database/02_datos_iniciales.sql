-- Datos de demostración. Ejecutar DESPUÉS de 01_esquema.sql.
USE facturas;

-- Usuario de demostración. La contraseña se guarda como hash BCrypt, nunca en claro.
-- REEMPLAZAR la línea siguiente por el INSERT generado (ver instrucciones en el README).
INSERT INTO usuarios (NombreUsuario, PasswordHash, Activo) VALUES ('admin', '$2a$11$A2H2uMEd9Mg4vwIAgHjTyOyQtowfWS7Xu7ULRAXq6UJ03Ez0VjxJW', 1);

INSERT INTO clientes (Nombre, Nit, Direccion, Activo) VALUES
  ('Consumidor Final', 'CF', NULL, 1),
  ('Distribuidora Maya, S.A.', '1234567-8', 'Zona 4, Ciudad de Guatemala', 1),
  ('Comercial Quetzal', '7654321-0', 'Zona 1, Jutiapa', 1);

INSERT INTO productos (Nombre, PrecioUnitario, Activo) VALUES
  ('Laptop 14 pulgadas', 7500.00, 1),
  ('Mouse inalámbrico', 150.00, 1),
  ('Teclado mecánico', 450.00, 1),
  ('Monitor 24 pulgadas', 1800.00, 1);

