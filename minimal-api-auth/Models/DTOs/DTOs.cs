using System.ComponentModel.DataAnnotations;
using MinimalAPIJWTAuth.Models;

public record UserDTO
{
	public string Id { get; set; }

	[Required]
	public string UserName { get; set; }

	[Required]
	public string FullName { get; set; }

	[Required]
	public string Email { get; set; }

	public int Age { get; set; }

	[Required]
	public string Role { get; set; }

	public string Address { get; set; }

	public User ToEntity()
	{
		return new User
		{
			Id = Guid.TryParse(Id, out Guid UserId) ? UserId : Guid.NewGuid(),
			UserName = UserName,
			FullName = FullName,
			Email = Email,
			Age = Age,
			Role = Role,
			Address = Address
		};
	}
}

public record UserCreateDTO
{
	[Required]
	[EmailAddress]
	public string Email { get; set; }

	[Required]
	[DataType(DataType.Password), MinLength(8)]
	public string Password { get; set; }

	[Required]
	[Compare("Password")]
	public string ConfirmPassword { get; set; }

	[Required]
	public string FullName { get; set; }

	public string Username { get; set; }
	public int Age { get; set; }

	[Required]
	public string Role { get; set; }

	public string Address { get; set; }
}

public record UserLoginDTO
{
	[Required]
	public string Login { get; set; }

	[Required]
	public string Password { get; set; }
}

public record TodoDTO(Guid id, string title, bool isDone, Guid assignedToId);
