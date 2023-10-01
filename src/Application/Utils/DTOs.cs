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
	public record TweetDTO(string OwnerAt,
						   string Text,
							IList<PostTweetDTO> Replies);

	public record PostTweetDTO(string OwnerAt,
							   string Text,
							   int ReplyToId,
							   DateTime PostTime);

}