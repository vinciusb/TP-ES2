using TwitterAPI.Application.Domain;

namespace TwitterAPI.Infrastructure.Persistence {
	public interface ITwitterRepository {
		/// ===============USER=================================================
		/// <summary>
		/// Get all the users registered in the user table.
		/// </summary>
		/// <returns> List of all users. </returns>
		Task<IEnumerable<User>> GetUsersAsync();
		/// <summary>
		/// Try to get a user based on its At or its Username depending on
		/// searchMode (true for Username).
		/// </summary>
		/// <param name="str"></param>
		/// <param name="searchMode"></param>
		/// <returns> A user that fits the filter specified. </returns>
		Task<User?> GetUserAsync(string str, bool searchMode);
		/// <summary>
		/// Try to get a user based on its database id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns> A user that has the same id as the specified. </returns>
		Task<User?> GetUserAsync(int id);
		/// <summary>
		/// Tries to create an user in the database.
		/// </summary>
		/// <param name="user"></param>
		/// <returns> Returns true with the user was created and false otherwise. </returns>
		Task<bool> CreateUserAsync(User user);
		Task UpdateUserAsync(int id, User user);
		Task DeleteUserAsync(int id);
		Task Tweet(int id, Tweet tweet);
		Task LikeTweet(User user, Tweet tweet, int tweetId);
		Task UnlikeTweet(User user, Tweet tweet, int tweetId);

		// Tweet table
	}
}