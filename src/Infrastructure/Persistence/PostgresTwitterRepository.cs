using Microsoft.EntityFrameworkCore;
using TwitterAPI.Domain;

namespace TwitterAPI.Infrastructure.Persistence {
	public class PostgresTwitterRepository : DbContext, ITwitterRepository {
		// DB
		private string ConnectionString { get; set; }
		public DbSet<User> Users;

		public PostgresTwitterRepository(string connectionString) {
			ConnectionString = connectionString;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			optionsBuilder.UseNpgsql(ConnectionString);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			// User
			modelBuilder.Entity<User>().ToTable("Users").HasKey(u => u.Id);

			modelBuilder.Entity<User>(e => {
				e.Property(u => u.Color).HasMaxLength(3);
				e.Property(u => u.At).HasMaxLength(20);
				e.Property(u => u.Username).HasMaxLength(25);
				e.Property(u => u.Bio).HasMaxLength(144);
				e.Property(u => u.City).HasMaxLength(30);
				e.Property(u => u.Country).HasMaxLength(30);
			});

			// Tweet
			modelBuilder.Entity<Tweet>().ToTable("Tweets").HasKey(t => t.Id);

			modelBuilder.Entity<Tweet>(e => {
				e.Property(t => t.Text).HasMaxLength(144);
			});

			modelBuilder
				.Entity<Tweet>()
				.HasOne(e => e.Owner)
				.WithOne()
				.HasForeignKey<Tweet>("OwnerId")
				.IsRequired();
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