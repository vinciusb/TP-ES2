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
		public static Tweet AsTweet(this PostTweetDTO tweet,
									User owner,
									Tweet? replyTo) {
			return new Tweet {
				Id = 0,
				Owner = owner,
				Text = tweet.Text,
				Replies = new List<Tweet>(),
				ReplyTo = replyTo,
				Likes = 0,
				PostTime = tweet.PostTime,
			};
		}

		public static SimpleTweetDTO AsSimpleDTO(this Tweet tweet) {
			return new SimpleTweetDTO(tweet.Id,
			 						  tweet.Owner.At,
									  tweet.Text,
									  tweet.PostTime);
		}

		public static FullNoParentingTweetDTO AsNoParentingTweetDTO(this Tweet tweet,
																	IEnumerable<FullNoParentingTweetDTO> tweetReplies) {
			return new FullNoParentingTweetDTO(tweet.Id,
											   tweet.Owner.At,
											   tweet.Text,
											   tweet.PostTime,
											   tweetReplies);
		}

		public static FullTweetDTO AsRecursiveTree(this Tweet rootTweet) {
			var replyTo = rootTweet.ReplyTo;
			var simplifiedParent = replyTo == null ?
									null :
									rootTweet.AsSimpleDTO();

			var rootTweetReplies = new List<FullNoParentingTweetDTO>();

			var treeIterationStack = new Stack<(Tweet, List<FullNoParentingTweetDTO>)>();
			treeIterationStack.Push((rootTweet, rootTweetReplies));

			while(treeIterationStack.Count != 0) {
				var (parentTweet, parentTweetReplies) = treeIterationStack.Pop();

				foreach(var reply in parentTweet.Replies) {
					var nextListOfReplies = new List<FullNoParentingTweetDTO>();
					var nextReply = reply.AsNoParentingTweetDTO(nextListOfReplies);

					parentTweetReplies.Add(nextReply);
					treeIterationStack.Push((reply, nextListOfReplies));
				}
			}

			return rootTweet.AsFullTweetDTO(simplifiedParent, rootTweetReplies);
		}

		public static FullTweetDTO AsFullTweetDTO(this Tweet rootTweet,
												  SimpleTweetDTO? simplifiedParent,
												  IEnumerable<FullNoParentingTweetDTO> tweetReplies) {
			return new FullTweetDTO(rootTweet.Id,
									rootTweet.Owner.At,
									rootTweet.Text,
									simplifiedParent,
									rootTweet.PostTime,
									tweetReplies);
		}
	}
}