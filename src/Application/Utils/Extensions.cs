using TwitterAPI.Application.Domain;
using TwitterAPI.Application.Utils.DTO;

namespace TwitterAPI.Application.Utils {
	public static class Extensions {
		// User
		public static User AsUser(this UserDTO user,
								  IList<Tweet> tweets,
								  IList<Tweet> likeHist) {
			return new User {
				Id = 0, // Since the ID is automatically created by the db
				Color = user.Color,
				At = user.At,
				Username = user.Username,
				Bio = user.Bio,
				City = user.City,
				Country = user.Country,
				BirthDate = user.BirthDate,
				Tweets = tweets,
				LikeHistory = likeHist,
			};
		}

		public static UserDTO AsDTO(this User user) {
			return new UserDTO(user.Color,
							   user.At,
							   user.Username,
							   user.Bio,
							   user.City,
							   user.Country,
							   user.BirthDate);
		}

		// Tweet
	}
}