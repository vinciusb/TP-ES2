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

		[HttpPost]
		public async Task<ActionResult> Tweet([FromQuery] string at, [FromBody] PostTweetDTO tweet) {
			// User? u = await repo.GetUserAsync(at, false);

			// if(u == null) return BadRequest();

			// Tweet? replyTo = ;

			// Tweet t = tweet.AsTweet();

			// // return CreatedAtAction(nameof(GetUsers), new { at = user.At }, user);
			// return CreatedAtAction("", new { }, t);
		}
	}
}