using Microsoft.AspNetCore.Identity;

namespace MinimalAPIJWTAuth.Models;

public class User : IdentityUser<Guid>
{
	public string FullName { get; set; }
	public int Age { get; set; }
	public string Role { get; set; }
	public string Address { get; set; }

	public User() { }

	public User(UserCreateDTO dto)
	{
		FullName = dto.FullName;
		Email = dto.Email;
		UserName = dto.Username;
		Age = dto.Age;
		Role = dto.Role;
		Address = dto.Address;
	}
}
