using Microsoft.EntityFrameworkCore;

namespace OrdersMinimalAPI.Models;

public class OrderDb : DbContext
{
    public OrderDb(DbContextOptions options) : base(options) { }

    public DbSet<Order> Orders => Set<Order>();
}