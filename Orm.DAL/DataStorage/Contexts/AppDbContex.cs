// Orm.DAL/DataStorage/Contexts/AppDbContex.cs
using Microsoft.EntityFrameworkCore;
using Orm.Core.Entities;
using Orm.DAL.Configurations;

namespace Orm.DAL.DataStorage.Contexts;

public class AppDbContex : DbContext
{
    public DbSet<MenuItem> MenuItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Table> Tables { get; set; }

    public AppDbContex() { } // Parameteresiz constructor eklendi

    public AppDbContex(DbContextOptions<AppDbContex> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Bağlantı dizesi doğrudan burada yapılandırıldı
            string connection = "Server=localhost,1433;Database=OrmMiniDb;User Id=sa;Password=Hebib123!;Encrypt=False;";
            optionsBuilder.UseSqlServer(connection);
        }
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TableConfig());
        modelBuilder.ApplyConfiguration(new OrderConfig());
        modelBuilder.ApplyConfiguration(new MenuItemConfig());
        modelBuilder.ApplyConfiguration(new OrderItemConfig());
        base.OnModelCreating(modelBuilder);
    }
}