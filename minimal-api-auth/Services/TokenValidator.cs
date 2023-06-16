using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MinimalAPIJWTAuth.Services;

public class TokenValidator
{
	private readonly byte[] _refreshTokenSecret;
	private readonly string _issuer;
	private readonly string _audience;

	public TokenValidator(IConfiguration config)
	{
		_refreshTokenSecret = Encoding.ASCII.GetBytes(config.GetValue<string>("Jwt:RefreshTokenSecret"));
		_issuer = config.GetValue<string>("Jwt:Issuer");
		_audience = config.GetValue<string>("Jwt:Audience");
	}

	public bool TryValidate(string refreshToken, out Guid tokenId)
	{
		var tokenHandler = new JwtSecurityTokenHandler();
		var tokenValidationParams = new TokenValidationParameters
		{
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(_refreshTokenSecret),
			ValidateIssuer = true,
			ValidateAudience = true,
			ClockSkew = TimeSpan.Zero,
			ValidAudience = _audience,
			ValidIssuer = _issuer,
		};

		try
		{
			tokenHandler.ValidateToken(refreshToken, tokenValidationParams, out SecurityToken token);
			var jwt = (JwtSecurityToken)token;
			var valid = Guid.TryParse(jwt.Id, out var id);
			tokenId = id;
			return valid;
		}
		catch (Exception)
		{
			tokenId = default;
			return false;
		}
	}
}
