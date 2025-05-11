using Microsoft.EntityFrameworkCore;
using FerreteriaAPI.Models;

namespace FerreteriaAPI.Data;
public class FerreteriaDbContext : DbContext
{
    public FerreteriaDbContext(DbContextOptions<FerreteriaDbContext> options) : base(options) {}

    public DbSet<Empleado> Empleados { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Factura> Facturas { get; set; }
    public DbSet<FacturaDetalle> FacturaDetalles { get; set; }
    public DbSet<User> Users => Set<User>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Factura>()
            .HasOne(f => f.Empleado)
            .WithMany()
            .HasForeignKey(f => f.EmpleadoId);

        modelBuilder.Entity<FacturaDetalle>()
            .HasOne(fd => fd.Factura)
            .WithMany(f => f.Detalles)
            .HasForeignKey(fd => fd.FacturaId);

        modelBuilder.Entity<FacturaDetalle>()
            .HasOne(fd => fd.Item)
            .WithMany()
            .HasForeignKey(fd => fd.ItemId);
    }
}
