using System.Text.Json;
using System.Text.Json.Serialization;
using MinimalAPIJWTAuth.Models;

namespace MinimalAPIJWTAuth.Utils;

public class UserConverter : JsonConverter<User>
{
	public override User Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var user = new User();
		var json = JsonDocument.Parse(reader.GetString()).RootElement.EnumerateObject();
		foreach (var property in json)
		{
			switch (property.Name)
			{
				case "id":
					user.Id = property.Value.GetGuid();
					break;
				case "fullName":
					user.FullName = property.Value.GetString();
					break;
				case "username":
					user.UserName = property.Value.GetString();
					break;
				case "email":
					user.Email = property.Value.GetString();
					break;
				case "age":
					user.Age = property.Value.GetInt32();
					break;
				case "role":
					user.Role = property.Value.GetString();
					break;
				case "address":
					user.Address = property.Value.GetString();
					break;
				default:
					break;
			}
		}
		return user;
	}

	public override void Write(Utf8JsonWriter writer, User user, JsonSerializerOptions options)
	{
		writer.WriteStartObject();
		writer.WriteString("id", user.Id.ToString());
		writer.WriteString("fullName", user.FullName);
		writer.WriteString("username", user.UserName);
		writer.WriteString("email", user.Email);
		writer.WriteNumber("age", user.Age);
		writer.WriteString("role", user.Role);
		writer.WriteString("address", user.Address);
		writer.WriteEndObject();
	}
}
