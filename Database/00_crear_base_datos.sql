-- Crea la base de datos y un usuario propio para la API.
-- Reemplazar <clave_bd> por una contraseña segura y usar la misma en la cadena de conexión.
CREATE DATABASE IF NOT EXISTS facturas CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

CREATE USER IF NOT EXISTS 'api_usrfac'@'localhost' IDENTIFIED BY '<clave_bd>';
GRANT ALL PRIVILEGES ON facturas.* TO 'api_usrfac'@'localhost';
FLUSH PRIVILEGES;
