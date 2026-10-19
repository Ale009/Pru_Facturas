# Facturas

Sistema de facturas. Primer módulo: inicio de sesión (usuario y contraseña).

## Tecnologías

- Backend: ASP.NET Core Web API (.NET 8, C#)
- Base de datos: MySQL con Entity Framework Core (Pomelo)
- Autenticación: JWT. Las contraseñas se guardan con hash BCrypt
- Frontend: React + Tailwind CSS (pendiente)

## Requisitos

- .NET SDK 8
- MySQL

## Paquetes instalados

```
dotnet add package Pomelo.EntityFrameworkCore.MySql --version '8.*'
dotnet add package Microsoft.EntityFrameworkCore.Design --version '8.*'
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version '8.*'
dotnet add package BCrypt.Net-Next
```

## Configuración de secretos (desarrollo)

Se usan *user-secrets*, ejecutados dentro de la carpeta del proyecto (donde está `Facturas.csproj`):

```
dotnet user-secrets init
dotnet user-secrets set 'ConnectionStrings:Default' 'Server=localhost;Port=3306;Database=facturas;User=<usuario_bd>;Password=<clave_bd>'
dotnet user-secrets set 'Jwt:Key' '<clave_larga_aleatoria_minimo_32_caracteres>'
dotnet user-secrets set 'Seed:AdminPassword' '<clave_inicial_del_admin>'
```
## Base de datos

Crear la base de datos y un usuario propio para la API.

```sql
CREATE DATABASE facturas CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

CREATE USER '<usuario_bd>'@'localhost' IDENTIFIED BY '<clave_bd>';
GRANT ALL PRIVILEGES ON facturas.* TO '<usuario_bd>'@'localhost';
FLUSH PRIVILEGES;
```

Las tablas se crean con migraciones de Entity Framework.

## Configuración no secreta (`appsettings.json`)

| Clave | Para qué sirve |
|---|---|
| `Jwt:Issuer` | Quién emite el token |
| `Jwt:Audience` | Para quién es el token |
| `Jwt:ExpiraMinutos` | Duración del token (60 minutos) |
| `Cors:OrigenPermitido` | Único origen autorizado para llamar a la API (el frontend en `http://localhost:5173`) |

Las claves de `Jwt:Key`, la cadena de conexión y la contraseña inicial del
administrador **no** están en este archivo: viven en user-secrets (desarrollo)
o en variables de entorno (producción).

## Estructura del proyecto

```
Facturas/
├── Controllers/   Endpoints HTTP (delgados, sin lógica de negocio)
├── Data/          AppDbContext (Entity Framework Core)
├── Dtos/          Objetos de entrada/salida de la API
├── Models/        Entidades que se mapean a tablas
└── Services/      Lógica de negocio (autenticación, tokens)
```

## Modelo de datos

Tabla `usuarios`:

| Columna | Tipo | Notas |
|---|---|---|
| Id | int | Clave primaria |
| NombreUsuario | varchar(50) | Índice único |
| PasswordHash | varchar(100) | Hash BCrypt, nunca la contraseña en claro |
| Activo | bool | Permite deshabilitar un usuario sin borrarlo |

Decisiones de diseño:
- Configuración del mapeo con Fluent API (`OnModelCreating`), para mantener
  las entidades libres de dependencias de infraestructura.
- Unicidad de `NombreUsuario` garantizada por índice en base de datos.
- Nombre de tabla en minúsculas: MySQL en Linux distingue mayúsculas en
  nombres de tabla.
