using CarritoComprasApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CarritoComprasApi.Data;

public class CarritoDbContext : DbContext
{
    public CarritoDbContext(DbContextOptions<CarritoDbContext> options)
        : base(options)
    {
    }

    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<ItemCarrito> ItemsCarrito => Set<ItemCarrito>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Producto>(producto =>
        {
            producto.Property(p => p.Nombre).HasMaxLength(100);
            producto.Property(p => p.Precio).HasPrecision(10, 2);

            producto.HasData(
                new Producto { Id = 1, Nombre = "Cuaderno", Precio = 25.50m },
                new Producto { Id = 2, Nombre = "Bolígrafo", Precio = 5.00m },
                new Producto { Id = 3, Nombre = "Mochila", Precio = 120.00m });
        });

        modelBuilder.Entity<ItemCarrito>(item =>
        {
            item.ToTable("ItemsCarrito", tabla => tabla.HasCheckConstraint(
                "CK_ItemsCarrito_Cantidad", "[Cantidad] BETWEEN 1 AND 99"));
            item.HasKey(i => i.ProductoId);
            item.Property(i => i.ProductoId).ValueGeneratedNever();
            item.Property(i => i.Version).IsRowVersion();

            item.HasOne(i => i.Producto)
                .WithMany()
                .HasForeignKey(i => i.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
