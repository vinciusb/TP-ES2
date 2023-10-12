using TwitterAPI.Application.Domain;
using TwitterAPI.Application.Utils.DTO;

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
		public Task UpdateUserProfileAsync(User user, UserDTO changedUser);
		/// <summary>
		/// Deletes a user.
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		Task DeleteUserAsync(User user);
		Task LikeTweetAsync(User user, Tweet tweet);
		Task UnlikeTweetAsync(User user, Tweet tweet);

		/// ===============TWEET================================================
		/// <summary>
		/// Function that gets all the tweets.
		/// </summary>
		/// <returns> All the tweets existing in the database. </returns>
		Task<IEnumerable<Tweet>> GetTweetsAsync();
		/// <summary>
		/// Get all the tweets of an certain user.
		/// </summary>
		/// <param name="at"></param>
		/// <returns> All tweets of an certain user. </returns>
		Task<IEnumerable<Tweet>> GetTweetsAsync(string at);
		/// <summary>
		/// Get all the tweets in a tweet sub tree (i.e. all its nested replies).
		/// </summary>
		/// <param name="id"></param>
		/// <returns> A tweet nested replies. </returns>
		Task<Tweet> GetTweetSubTreeAsync(int id);
		/// <summary>
		/// Get an timeline based on tweets relevance.
		/// </summary>
		/// <returns> All tweets in a certain order based on their relevance. </returns>
		Task<IEnumerable<Tweet>> GetTimelineAsync();
		/// <summary>
		/// Gets a specific tweet.
		/// </summary>
		/// <param name="id"></param>
		/// <returns> A specific tweet. </returns>
		Task<Tweet?> GetTweetAsync(int id);
		/// <summary>
		/// Publishes a tweet.
		/// </summary>
		/// <param name="tweet"></param>
		/// <returns></returns>
		Task TweetAsync(Tweet tweet);
		/// <summary>
		/// Deletes a tweet.
		/// </summary>
		/// <param name="tweet"></param>
		/// <returns></returns>
		Task DeleteTweetAsync(Tweet tweet);
	}
}