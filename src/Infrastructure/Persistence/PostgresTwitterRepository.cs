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
			ConfigurateUsersDB(modelBuilder);
			ConfigurateTweetsDB(modelBuilder);
		}

		private static void ConfigurateUsersDB(ModelBuilder modelBuilder) {
			modelBuilder.Entity<User>(e => {
				e.ToTable("Users").HasKey(u => u.Id);

				e.Property(u => u.Id).ValueGeneratedOnAdd();
				e.Property(u => u.Color).HasMaxLength(6);
				e.Property(u => u.At).HasMaxLength(20);
				e.Property(u => u.Username).HasMaxLength(25);
				e.Property(u => u.Bio).HasMaxLength(144);
				e.Property(u => u.City).HasMaxLength(30);
				e.Property(u => u.Country).HasMaxLength(30);
			});

			// Relations
			modelBuilder.Entity<User>()
						.HasMany(e => e.Tweets)
						.WithOne(e => e.Owner)
						.HasForeignKey("OwnerId")
						.IsRequired();
			modelBuilder.Entity<User>()
						.HasMany(e => e.LikeHistory)
						.WithMany()
						.UsingEntity(j => {
							j.ToTable("LikeHistory");
							j.Property("LikeHistoryId").HasColumnName("TweetId");
						});
		}

		private static void ConfigurateTweetsDB(ModelBuilder modelBuilder) {
			modelBuilder.Entity<Tweet>(e => {
				e.ToTable("Tweets").HasKey(t => t.Id);

				e.Property(t => t.Text).HasMaxLength(144);
			});

			// Relations
			modelBuilder.Entity<Tweet>()
						.HasOne(t => t.ReplyTo)
						.WithMany();
			modelBuilder.Entity<Tweet>()
						.HasMany(t => t.Replies)
						.WithMany()
						.UsingEntity(t => {
							t.ToTable("TweetsReplies");
							t.Property("RepliesId").HasColumnName("ReplyId");
							t.Property("TweetId").HasColumnName("ParentId");
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
			var rootTweet = await GetTweetAsync(id);

			await LoadRepliesRecursively(rootTweet);

			return rootTweet;
		}

		private async Task LoadRepliesRecursively(Tweet root) {
			foreach(var reply in root.Replies) {
				// Since efcore works with a reference system, when we load
				// an full included tweet reference, the current reference
				// of the tweet just get updated, then we do not need to
				// substitute t.Replies
				await GetTweetAsync(reply.Id);
				await LoadRepliesRecursively(reply);
			}
		}
		public async Task<IEnumerable<Tweet>> GetTimelineAsync() {
			var rootTweets = Tweets.Where(t => t.ReplyTo == null).ToList();
			var rootTrees = new List<Tweet>();

			foreach(var rootTweet in rootTweets) {
				rootTrees.Add(await GetTweetSubTreeAsync(rootTweet.Id));
			}

			var tweetIdToScoreMap = new Dictionary<int, (int, int)>();
			foreach(var tree in rootTrees) {
				// Gets a dictionary mapping [Tweet Id] -> [numer of likes, number of children tweets]
				GetTreeDescriptionRecursively(tree, tweetIdToScoreMap);
			}

			var tweetsDict = (await GetTweetsAsync()).ToDictionary(t => t.Id, t => t);

			var currentDate = DateTime.Now;
			var timelineIds = tweetIdToScoreMap
								.Select(pair =>
									(pair.Key, CalculateTweetScore(pair.Value,
																   currentDate,
																   tweetsDict[pair.Key].PostTime
																   ))
								)
								.OrderByDescending(scorePair => scorePair.Item2)
								.Select(pair => pair.Key);

			return timelineIds.Select(id => tweetsDict[id]);
		}

		private int GetTreeDescriptionRecursively(Tweet root, IDictionary<int, (int, int)> dict) {
			int totalChildrenNumber = 0;

			foreach(var reply in root.Replies) {
				var howManyChildren = GetTreeDescriptionRecursively(reply, dict);
				totalChildrenNumber += howManyChildren + 1;
			}

			dict.Add(root.Id, (root.Likes, totalChildrenNumber));
			return totalChildrenNumber;
		}

		private static int CalculateTweetScore((int, int) likesAndReplies,
										DateTime currentDate,
										DateTime tweetDate) {
			// This variables try to balance the timeline in terms of how relevance is built.
			const int LIKE_WEIGHT = 100;
			const int REPLY_WEIGHT = 70;
			// ATTENTION: INCREASING THIS VARIABLE TOO MUCH WILL TURN THE SOCIAL MEDIA TOO IMMEDIATIST
			const int TWEET_DEPRECATION_VALUE_PER_HOUR = 3;
			const int HOUR_SATURATION = 24;

			var (likes, replies) = likesAndReplies;
			var timeSpan = currentDate - tweetDate;
			var howManyHoursHavePassed = Math.Min(Convert.ToInt32(timeSpan.TotalHours), HOUR_SATURATION);

			return (likes * LIKE_WEIGHT) +
				   (replies * REPLY_WEIGHT) -
				   (howManyHoursHavePassed * TWEET_DEPRECATION_VALUE_PER_HOUR);
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