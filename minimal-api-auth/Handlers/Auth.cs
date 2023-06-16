using Microsoft.EntityFrameworkCore;
using MinimalAPIJWTAuth.Services;
using static Microsoft.AspNetCore.Http.Results;

namespace MinimalAPIJWTAuth.Handlers;

public static class Auth
{
	internal static async Task<IResult> RefreshTokenAsync
	(
		HttpRequest request,
		HttpResponse response,
		TokenRepository repository,
		TokenValidator validator,
		TokenGenerator tokens,
		AppDbContext db
	)
	{
		var refreshToken = request.Cookies["refresh_token"];

		if (string.IsNullOrWhiteSpace(refreshToken))
			return BadRequest("Please include a refresh token in the request.");

		var tokenIsValid = validator.TryValidate(refreshToken, out var tokenId);
		if (!tokenIsValid) return BadRequest("Invalid refresh token.");

		var token = await repository.Tokens.Where(token => token.Id == tokenId).FirstOrDefaultAsync();
		if (token is null) return BadRequest("Refresh token not found.");

		var user = await db.Users.Where(u => u.Id == token.UserId).FirstOrDefaultAsync();
		if (user is null) return BadRequest("User not found.");

		var accessToken = tokens.GenerateAccessToken(user);
		var (newRefreshTokenId, newRefreshToken) = tokens.GenerateRefreshToken();

		repository.Tokens.Remove(token);
		await repository.Tokens.AddAsync(new Token { Id = newRefreshTokenId, UserId = user.Id });
		await repository.SaveChangesAsync();

		response.Cookies.Append("refresh_token", newRefreshToken, new CookieOptions
		{
			Expires = DateTime.Now.AddDays(1),
			HttpOnly = true,
			IsEssential = true,
			MaxAge = new TimeSpan(1, 0, 0, 0),
			Secure = true,
			SameSite = SameSiteMode.Strict
		});

		return Ok(accessToken);
	}

	internal static async Task<IResult> SignOutAsync
	(
		HttpRequest request,
		HttpResponse response,
		TokenRepository repository,
		TokenValidator validator
	)
	{
		var refreshToken = request.Cookies["refresh_token"];

		if (string.IsNullOrWhiteSpace(refreshToken))
			return BadRequest("Please include a refresh token in the request.");

		var tokenIsValid = validator.TryValidate(refreshToken, out var tokenId);
		if (!tokenIsValid) return BadRequest("Invalid refresh token.");

		var token = await repository.Tokens.Where(token => token.Id == tokenId).FirstOrDefaultAsync();
		if (token is null) return BadRequest("Refresh token not found.");

		repository.Tokens.Remove(token);
		await repository.SaveChangesAsync();

		response.Cookies.Delete("refresh_token");
		return NoContent();
	}
}
