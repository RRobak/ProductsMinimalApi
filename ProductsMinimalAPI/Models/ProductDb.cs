using Microsoft.EntityFrameworkCore;

namespace ProductsMinimalAPI.Models;

public class ProductDb : DbContext
{
    public ProductDb(DbContextOptions options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();
}