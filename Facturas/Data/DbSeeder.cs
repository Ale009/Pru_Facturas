using Facturas.Models;
using Microsoft.EntityFrameworkCore;

namespace Facturas.Data;

/// <summary>Aplica migraciones y carga datos iniciales. Solo se ejecuta en desarrollo.</summary>
public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider services, IConfiguration config)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await db.Database.MigrateAsync();

        // La contraseña del admin viene de user-secrets, nunca del código.
        var clave = config["Seed:AdminPassword"];
        if (!await db.Usuarios.AnyAsync() && !string.IsNullOrWhiteSpace(clave))
        {
            db.Usuarios.Add(new Usuario
            {
                NombreUsuario = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(clave)
            });
        }

        if (!await db.Clientes.AnyAsync())
        {
            db.Clientes.AddRange(
                new Cliente { Nombre = "Consumidor Final", Nit = "CF" },
                new Cliente { Nombre = "Distribuidora Maya, S.A.", Nit = "1234567-8", Direccion = "Zona 4, Ciudad de Guatemala" },
                new Cliente { Nombre = "Comercial Quetzal", Nit = "7654321-0", Direccion = "Zona 1, Jutiapa" });
        }

        if (!await db.Productos.AnyAsync())
        {
            db.Productos.AddRange(
                new Producto { Nombre = "Laptop 14 pulgadas", PrecioUnitario = 7500.00m },
                new Producto { Nombre = "Mouse inalámbrico", PrecioUnitario = 150.00m },
                new Producto { Nombre = "Teclado mecánico", PrecioUnitario = 450.00m },
                new Producto { Nombre = "Monitor 24 pulgadas", PrecioUnitario = 1800.00m });
        }

        await db.SaveChangesAsync();
    }
}