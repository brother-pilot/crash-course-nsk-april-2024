using Market.Models;
using Microsoft.EntityFrameworkCore;

namespace Market.DAL;

internal sealed class RepositoryContext : DbContext
{
    public RepositoryContext()
    {
        Database.EnsureCreated();
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<User> Users => Set<User>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appSettings.json")
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "Properties"))
            .Build();

        var connectionString = config.GetConnectionString("DefaultConnection");
        optionsBuilder.UseSqlite(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var dataInitializer = new DataInitializer();
        modelBuilder.Entity<Product>().HasData(dataInitializer.GetSeedProducts());
        
        modelBuilder.Entity<Cart>().HasKey(c => c.CustomerId);
        modelBuilder.Entity<Cart>().Property(c => c.ProductIds).HasColumnType("TEXT")
            .HasConversion(
                ids => string.Join(';', ids), 
                s => s.Split(';', StringSplitOptions.RemoveEmptyEntries).Select(Guid.Parse).ToList());
        modelBuilder.Entity<Cart>().HasData(dataInitializer.GetSeedCarts());

        modelBuilder.Entity<User>().HasIndex(s => s.Login).IsUnique();
        modelBuilder.Entity<User>().HasData(dataInitializer.GetSeedUsers());
        /*modelBuilder.Entity<Product>().HasData(DataInitializer.InitializeProducts());
        //страхуемся для добавления свойства CustomerId, задаем явно первичный ключ
        //modelBuilder.Entity<Cart>().HasKey(item=>item.CustomerId);
        //трекер не записывал изменения списка т.к. внутрь изменяемого списка не смотрел думал что список не меняется
        modelBuilder.Entity<Cart>().Property(c => c.ProductIds).HasColumnType("TEXT")
            .HasConversion(
                ids => string.Join(';', ids), 
                s => s.Split(';', StringSplitOptions.RemoveEmptyEntries).Select(Guid.Parse).ToList());
        //иницилизируем начальными значениями БД
        modelBuilder.Entity<Cart>().HasData(DataInitializer.InitializeCarts());*/
    }
}