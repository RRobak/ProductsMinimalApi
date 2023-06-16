using Microsoft.AspNetCore.Identity;
using MinimalAPIJWTAuth.Models;
using MinimalAPIJWTAuth.Services;
using MinimalAPIJWTAuth.Utils;
using MiniValidation;
using static Microsoft.AspNetCore.Http.Results;

namespace MinimalAPIJWTAuth.Handlers;

public static class Users
{
	internal static async Task<IResult> SignUpAsync(AppDbContext db, UserManager<User> users, UserCreateDTO user)
	{
		if (user is null) return BadRequest();
		if (!MiniValidator.TryValidate(user, out var errors)) return BadRequest(errors);

		if (users.Users.Any(u => u.Email == user.Email))
			return Conflict("Invalid `email`: A user with this email address already exists.");

		if (string.IsNullOrWhiteSpace(user.Username))
		{
			user.Username = string.Join('_', user.FullName.Split(' ')).ToLower();
			if (users.Users.Any(u => u.UserName == user.Username))
				user.Username = user.Username + '_' + Guid.NewGuid().ToString("N").Substring(0, 4);
		}
		else if (users.Users.Any(u => u.UserName == user.Username))
			return Conflict("Invalid `username`: A user with this username already exists.");

		var newser = new User(user);
		var result = await users.CreateAsync(newser, user.Password);
		if (!result.Succeeded) return BadRequest(result.Errors);

		return Created($"/users/{newser.Id}", newser);
	}

	internal static async Task<IResult> SignInAsync
	(
		UserManager<User> users,
		TokenGenerator tokens,
		TokenRepository repository,
		UserLoginDTO credentials,
		HttpResponse response
	)
	{
		if (credentials is null) return BadRequest();
		if (!MiniValidator.TryValidate(credentials, out var errors)) return BadRequest(errors);

		var isUsingEmailAddress = EmailAddressValidator.TryValidate(credentials.Login, out var _);
		var user = isUsingEmailAddress
			? await users.FindByEmailAsync(credentials.Login)
			: await users.FindByNameAsync(credentials.Login);
		if (user is null) return NotFound("User with this email address not found.");

		var result = await users.CheckPasswordAsync(user, credentials.Password);
		if (!result) return BadRequest("Incorrect password.");

		var accessToken = tokens.GenerateAccessToken(user);
		var (refreshTokenId, refreshToken) = tokens.GenerateRefreshToken();

		await repository.Tokens.AddAsync(new Token { Id = refreshTokenId, UserId = user.Id });
		await repository.SaveChangesAsync();

		response.Cookies.Append("refresh_token", refreshToken, new CookieOptions
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
}
