using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace MinimalAPIJWTAuth.Services;

public class TokenRepository : DbContext
{
	public DbSet<Token> Tokens { get; set; }

	public TokenRepository(DbContextOptions<TokenRepository> options) : base(options) { }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Token>().HasIndex(b => b.UserId);
	}
}

public record Token
{
	[Key]
	public Guid Id { get; init; }
	public Guid UserId { get; init; }
}
