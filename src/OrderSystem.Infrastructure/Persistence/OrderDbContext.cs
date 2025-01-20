using Microsoft.EntityFrameworkCore;
using OrderSystem.Domain.Entities;
using OrderSystem.Infrastructure.Configurations;


namespace OrderSystem.Infrastructure.Persistence
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderDbContext).Assembly);

            modelBuilder.Entity<Client>().HasData(
                new Client{Id = Guid.NewGuid(), Name = "Ben"},
                new Client{Id = Guid.NewGuid(), Name = "Ferreira"}
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}