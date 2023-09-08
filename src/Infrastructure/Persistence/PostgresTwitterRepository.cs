using Microsoft.EntityFrameworkCore;
using TwitterAPI.Domain;

namespace TwitterAPI.Infrastructure.Persistence {
	public class PostgresTwitterRepository : DbContext, ITwitterRepository {
		// DB

		// USER
		public Task CreateUserAsync(User user) {
			throw new NotImplementedException();
		}

		public Task DeleteUserAsync(int id) {
			throw new NotImplementedException();
		}

		public Task<User> GetUserAsync(string str, bool searchMode) {
			throw new NotImplementedException();
		}

		public Task<User> GetUserAsync(int id) {
			throw new NotImplementedException();
		}

		public Task<IEnumerable<User>> GetUsersAsync() {
			throw new NotImplementedException();
		}

		public Task LikeTweet(User user, Tweet tweet, int tweetId) {
			throw new NotImplementedException();
		}

		public Task Tweet(int id, Tweet tweet) {
			throw new NotImplementedException();
		}

		public Task UnlikeTweet(User user, Tweet tweet, int tweetId) {
			throw new NotImplementedException();
		}

		public Task UpdateUserAsync(int id, User user) {
			throw new NotImplementedException();
		}
	}
}