using Microsoft.EntityFrameworkCore;
using Order_Web_Api.Entities;

namespace Order_Web_Api.Context
{
	public class OrderApiDbContext : DbContext
	{
        public OrderApiDbContext(DbContextOptions options) : base(options)  
        {
            
        }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems  { get; set; }

    }
}
