using Microsoft.EntityFrameworkCore;
using TwitterAPI.Application.Domain;

namespace TwitterAPI.Infrastructure.Persistence {
	public class PostgresTwitterRepository : DbContext, ITwitterRepository {
		// DB
		private string ConnectionString { get; set; }
		public DbSet<User> Users { get; set; } = null!;

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
				e.Property(u => u.Id).ValueGeneratedOnAdd();
				e.Property(u => u.Color).HasMaxLength(6);
				e.Property(u => u.At).HasMaxLength(20);
				e.Property(u => u.Username).HasMaxLength(25);
				e.Property(u => u.Bio).HasMaxLength(144);
				e.Property(u => u.City).HasMaxLength(30);
				e.Property(u => u.Country).HasMaxLength(30);
			});
			modelBuilder
				.Entity<User>()
				.HasMany(e => e.Tweets)
				.WithOne(e => e.Owner)
				.HasForeignKey("OwnerId")
				.IsRequired();
			modelBuilder
				.Entity<User>()
				.HasMany(e => e.LikeHistory)
				.WithMany()
				.UsingEntity(j => {
					j.ToTable("LikeHistory");
					j.Property("LikeHistoryId").HasColumnName("TweetId");
				});

			modelBuilder
				.Entity<Tweet>()
				.HasOne(t => t.ReplyTo)
				.WithOne();
			modelBuilder
				.Entity<Tweet>()
				.HasMany(t => t.Replies)
				.WithMany()
				.UsingEntity(t => {
					t.ToTable("TweetsReplies");
					t.Property("RepliesId").HasColumnName("ReplyId");
					t.Property("TweetId").HasColumnName("ParentId");
				});

			// Tweet
			modelBuilder.Entity<Tweet>().ToTable("Tweets").HasKey(t => t.Id);

			modelBuilder.Entity<Tweet>(e => {
				e.Property(t => t.Text).HasMaxLength(144);
			});
		}

		// ======== USER =======================================================
		public async Task<IEnumerable<User>> GetUsersAsync() {
			return await Users.ToListAsync();
		}

		public async Task<User?> GetUserAsync(string str, bool searchMode) {
			return await Users.Where(
							searchMode ?
							  (User u) => u.Username == str :
							  (User u) => u.At == str
						)
						.FirstAsync();
		}

		public async Task<User?> GetUserAsync(int id) {
			return await Users.Where(u => u.Id == id).FirstAsync();
		}

		public async Task<bool> CreateUserAsync(User user) {
			// If there is no user with this at
			if(!await Users.AnyAsync(u => u.At == user.At)) {
				await Users.AddAsync(user);
				await SaveChangesAsync();
				return true;
			}
			return false;
		}

		public async Task DeleteUserAsync(int id) {
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