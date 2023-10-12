using Microsoft.EntityFrameworkCore;
using TwitterAPI.Application.Domain;
using TwitterAPI.Application.Utils.DTO;

namespace TwitterAPI.Infrastructure.Persistence {
	public class PostgresTwitterRepository : DbContext, ITwitterRepository {
		// DB
		private string ConnectionString { get; set; }
		public DbSet<User> Users { get; set; } = null!;
		public DbSet<Tweet> Tweets { get; set; } = null!;

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
				.WithMany();
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
			return await Users
						 .Include(u => u.Tweets)
						 .Include(u => u.LikeHistory)
						 .ToListAsync();
		}

		public async Task<User?> GetUserAsync(string str, bool searchMode) {
			return await Users.Where(
							searchMode ?
								(User u) => u.Username == str :
								(User u) => u.At == str
						)
						.Include(u => u.Tweets)
						.Include(u => u.LikeHistory)
						.FirstOrDefaultAsync();
		}

		public async Task<User?> GetUserAsync(int id) {
			return await Users
						 .Include(u => u.Tweets)
						 .Include(u => u.LikeHistory)
						 .Where(u => u.Id == id)
						 .FirstOrDefaultAsync();
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

		public async Task UpdateUserProfileAsync(User user, UserDTO changedUser) {
			user.Color = changedUser.Color;
			user.Username = changedUser.Username;
			user.Bio = changedUser.Bio;
			user.City = changedUser.City;
			user.Country = changedUser.Country;
			user.BirthDate = changedUser.BirthDate;

			await SaveChangesAsync();
		}

		public async Task DeleteUserAsync(User user) {
			Users.Remove(user);
			await SaveChangesAsync();
		}

		public async Task LikeTweetAsync(User user, Tweet tweet) {
			user.LikeHistory.Add(tweet);
			tweet.Likes++;

			await SaveChangesAsync();
		}

		public async Task UnlikeTweetAsync(User user, Tweet tweet) {
			user.LikeHistory.Remove(tweet);
			tweet.Likes--;

			await SaveChangesAsync();
		}

		// ======== TWEET ======================================================

		public async Task<IEnumerable<Tweet>> GetTweetsAsync() {
			return await Tweets
						 .Include(t => t.Owner)
						 .Include(t => t.Replies)
						 .Include(t => t.ReplyTo)
						 .ToListAsync();
		}

		public async Task<IEnumerable<Tweet>> GetTweetsAsync(string at) {
			return await Tweets
						 .Where(t => t.Owner.At == at)
						 .Include(t => t.Owner)
						 .Include(t => t.Replies)
						 .Include(t => t.ReplyTo)
						 .ToListAsync();
		}

		public async Task<Tweet?> GetTweetAsync(int id) {
			return await Tweets
						 .Where(t => t.Id == id)
						 .Include(t => t.Owner)
						 .Include(t => t.Replies)
						 .Include(t => t.ReplyTo)
						 .FirstOrDefaultAsync();
		}

		public async Task<Tweet> GetTweetSubTreeAsync(int id) {
			var x = await Tweets
						 .Where(t => t.Id == id)
						 .Include(t => t.Owner)
						 .Include(t => t.Replies)
						 .Include(t => t.ReplyTo)
						 .FirstOrDefaultAsync();
			if(x == null) throw new Exception("Tweet does not exists");

			var stack = new Stack<Tweet>();
			stack.Push(x);

			while(stack.Count() != 0) {
				var t = stack.Pop();

				foreach(var reply in t.Replies) {
					// Since efcore works with a reference system, when we load
					// an full included tweet reference, the current reference
					// of the tweet just get updated, then we do not need to
					// substitute t.Replies
					await Tweets
							.Where(t => t.Id == reply.Id)
							.Include(t => t.Owner)
							.Include(t => t.Replies)
							.Include(t => t.ReplyTo)
							.FirstOrDefaultAsync();

					stack.Push(reply);
				}
			}

			return x;
		}

		public async Task TweetAsync(Tweet tweet) {
			await Tweets.AddAsync(tweet);

			Tweet? parentTweet = tweet.ReplyTo;
			if(parentTweet != null)
				parentTweet.Replies.Add(tweet);

			await SaveChangesAsync();
		}

		public async Task DeleteTweetAsync(Tweet tweet) {
			Tweets.Remove(tweet);
			await SaveChangesAsync();
		}
	}
}