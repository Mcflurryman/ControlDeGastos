using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using ControlDeGastos.Models;

namespace ControlDeGastos.Data;
public class ControlGastosContext : DbContext
{
    public ControlGastosContext(DbContextOptions<ControlGastosContext> options)
        : base(options)
    {
    }

    public DbSet<CategoriaModel> Categoria { get; set; }
    public DbSet<GastoModel> Gastos { get; set; }

    public DbSet<GastoAutomaticoModel> GastoAutomatico { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GastoModel>()
            .Property(g => g.Importe)
            .HasPrecision(10, 2);
    }
}
