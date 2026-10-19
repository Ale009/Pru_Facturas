using Facturas.Models;
using Microsoft.EntityFrameworkCore;

namespace Facturas.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<Factura> Facturas => Set<Factura>();
    public DbSet<FacturaDetalle> FacturaDetalles => Set<FacturaDetalle>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>(e =>
        {
            e.ToTable("usuarios");
            e.HasKey(u => u.Id);
            e.Property(u => u.NombreUsuario).IsRequired().HasMaxLength(50);
            e.Property(u => u.PasswordHash).IsRequired().HasMaxLength(100);
            e.HasIndex(u => u.NombreUsuario).IsUnique();
        });

        modelBuilder.Entity<Cliente>(e =>
        {
            e.ToTable("clientes");
            e.Property(c => c.Nombre).IsRequired().HasMaxLength(100);
            e.Property(c => c.Nit).IsRequired().HasMaxLength(20);
            e.Property(c => c.Direccion).HasMaxLength(200);
        });

        modelBuilder.Entity<Producto>(e =>
        {
            e.ToTable("productos");
            e.Property(p => p.Nombre).IsRequired().HasMaxLength(100);
            e.Property(p => p.PrecioUnitario).HasPrecision(18, 2);
        });

        modelBuilder.Entity<Factura>(e =>
        {
            e.ToTable("facturas");
            e.Property(f => f.Total).HasPrecision(18, 2);
            e.Property(f => f.Estado).HasConversion<string>().HasMaxLength(20);
            e.Property(f => f.MotivoAnulacion).HasMaxLength(200);

            e.HasOne(f => f.Cliente).WithMany()
                .HasForeignKey(f => f.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(f => f.Usuario).WithMany()
                .HasForeignKey(f => f.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasMany(f => f.Detalles).WithOne(d => d.Factura)
                .HasForeignKey(d => d.FacturaId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<FacturaDetalle>(e =>
        {
            e.ToTable("factura_detalles");
            e.Property(d => d.PrecioUnitario).HasPrecision(18, 2);
            e.Property(d => d.Subtotal).HasPrecision(18, 2);

            e.HasOne(d => d.Producto).WithMany()
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}