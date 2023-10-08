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
	// Used for tweets that we don't want to get too deep (don't care about parenting)
	// Since the Id is given, if the user wants more info about it, just click to expand and require it's full info
	public record SimpleTweetDTO(int Id,
								 string OwnerAt,
								 string Text,
								 DateTime PostTime);

	public record SemiTweetDTO(int Id,
							   string OwnerAt,
							   string Text,
							   SimpleTweetDTO? ReplyTo,
							   DateTime PostTime,
							   IEnumerable<SimpleTweetDTO> Replies);

	public record FullTweetDTO(int Id,
							   string OwnerAt,
							   string Text,
							   SimpleTweetDTO? ReplyTo,
							   DateTime PostTime,
							   IEnumerable<FullNoParentingTweetDTO> Replies);

	public record FullNoParentingTweetDTO(int Id,
										  string OwnerAt,
										  string Text,
										  DateTime PostTime,
										  IEnumerable<FullNoParentingTweetDTO> Replies);


	public record PostTweetDTO(string OwnerAt,
							   string Text,
							   int? ReplyToId,
							   DateTime PostTime);

}