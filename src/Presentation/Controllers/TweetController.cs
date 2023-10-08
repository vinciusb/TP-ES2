using Microsoft.AspNetCore.Mvc;
using TwitterAPI.Application.Domain;
using TwitterAPI.Application.Utils;
using TwitterAPI.Application.Utils.DTO;
using TwitterAPI.Infrastructure.Persistence;

namespace TwitterAPI.Presentation.Controllers {
	[ApiController]
	[Route("api/[controller]")]
	public class TweetController : ControllerBase {
		private readonly ITwitterRepository repo;
		private readonly ILogger<UserController> _logger;

		public TweetController(ITwitterRepository repo, ILogger<UserController> logger) {
			this.repo = repo;
			_logger = logger;
		}

		[HttpGet]
		public async Task<IEnumerable<SimpleTweetDTO?>> GetTweet([FromQuery] string? at, [FromQuery] int? id) {
			IEnumerable<Tweet> tweets;

			// Get by identifier
			if(at != null) tweets = await repo.GetTweetsAsync(at);
			else if(id != null) {
				var t = await repo.GetTweetAsync((int)id);
				tweets = t == null ? new List<Tweet>() { } : new List<Tweet>() { t };
			} else tweets = await repo.GetTweetsAsync();

			return tweets.Select(t => t.AsSimpleDTO());
		}

		[HttpGet("subtree")]
		public async Task<ActionResult> GetTweetSubtree([FromQuery] int id) {
			return BadRequest();
		}

		[HttpPost]
		public async Task<ActionResult> Tweet([FromBody] PostTweetDTO tweet) {
			User? owner = await repo.GetUserAsync(tweet.OwnerAt, false);

			if(owner == null) return BadRequest();

			Tweet? replyTo = tweet.ReplyToId == null ? null : await repo.GetTweetAsync((int)tweet.ReplyToId);

			Tweet toCreateTweet = tweet.AsTweet(owner, replyTo);
			await repo.TweetAsync(toCreateTweet);

			return CreatedAtAction(nameof(GetTweet), new { Id = toCreateTweet.Id }, toCreateTweet.AsSimpleDTO());
		}
	}
}