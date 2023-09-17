namespace TwitterAPI.Application.Utils.DTO {
	// User
	public record UserDTO(string Color,
						  string At,
						  string Username,
						  string Bio,
						  string City,
						  string Country,
						  DateOnly? BirthDate);

	// Tweet
}