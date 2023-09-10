using Microsoft.EntityFrameworkCore;
using TwitterAPI.Domain;

namespace TwitterAPI.Infrastructure.Persistence {
	public class PostgresTwitterRepository : DbContext, ITwitterRepository {
		// DB
		public DbSet<User> Users;

		public PostgresTwitterRepository(DbContextOptions<PostgresTwitterRepository> opt) : base(opt) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.Entity<User>();
		}

		// USER
		public async Task<bool> CreateUserAsync(User user) {
			var users = Users.Where(u => u.At == user.At);
			if(users == null) {
				await Users.AddAsync(user);
				await SaveChangesAsync();
				return true;
			}
			return false;
		}

		public async Task DeleteUserAsync(int id) {
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