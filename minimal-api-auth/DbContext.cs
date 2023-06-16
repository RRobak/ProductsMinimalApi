using Microsoft.EntityFrameworkCore;
using MinimalAPIJWTAuth.Models;

namespace MinimalAPIJWTAuth;

public class AppDbContext : DbContext
{
	public DbSet<User> Users { get; set; }

	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.Entity<User>();
	}
}
