using Microsoft.EntityFrameworkCore;
using restaurant_management_system._2.Domain.Entities;

namespace restaurant_management_system._2.Infrastructure.Data
{
    public class RestaurantDbContext : DbContext
    {
        public DbSet<Table> Tables { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
              @"Server=localhost\SQLEXPRESS02;Database=RestaurantManagementSystemDb;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}