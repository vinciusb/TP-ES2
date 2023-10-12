using Microsoft.AspNetCore.Mvc;
using TwitterAPI.Infrastructure.Persistence;
using TwitterAPI.Application.Utils.DTO;
using TwitterAPI.Application.Domain;
using TwitterAPI.Application.Utils;

namespace TwitterAPI.Presentation.Controllers {
	[ApiController]
	[Route("api/[controller]")]
	public class UserController : ControllerBase {
		private readonly ITwitterRepository repo;
		private readonly ILogger<UserController> _logger;

		public UserController(ITwitterRepository repo, ILogger<UserController> logger) {
			this.repo = repo;
			_logger = logger;
		}

		[HttpGet]
		public async Task<IEnumerable<UserDTO?>> GetUsers([FromQuery] string? at, [FromQuery] string? username, [FromQuery] int? id) {
			User? user;

			// Get by identifier
			if(at != null) user = await repo.GetUserAsync(at, false);
			else if(username != null) user = await repo.GetUserAsync(username, true);
			else if(id != null) user = await repo.GetUserAsync((int)id);
			// Get all
			else return (await repo.GetUsersAsync()).Select(u => u.AsDTO());

			if(user == null) return new List<UserDTO?>();
			return new List<UserDTO?>() { user?.AsDTO() };
		}

		[HttpPost]
		public async Task<ActionResult> PostUser([FromBody] UserDTO user) {
			User u = user.AsUser(new List<Tweet>(), new List<Tweet>());

			if(!await repo.CreateUserAsync(u)) return BadRequest();

			return CreatedAtAction(nameof(GetUsers), new { at = user.At }, user);
		}

		[HttpPut]
		public async Task<ActionResult> ChangeBasicInfos([FromBody] UserDTO modifiedUser) {
			User? u = await repo.GetUserAsync(modifiedUser.At, false);

			if(u == null) return BadRequest();

			await repo.UpdateUserProfileAsync(u, modifiedUser);
			return NoContent();
		}

		[HttpPut("likes/like")]
		public async Task<ActionResult> LikeTweet([FromQuery] string at, [FromQuery] int tweetId) {
			User? user = await repo.GetUserAsync(at, false);
			if(user == null) return BadRequest();

			Tweet? tweet = await repo.GetTweetAsync(tweetId);
			if(tweet == null) return BadRequest();

			bool userHasAlreadyLiked = user.LikeHistory.Any(t => t.Id == tweet.Id);
			if(userHasAlreadyLiked)
				return BadRequest();

			await repo.LikeTweetAsync(user, tweet);
			return NoContent();
		}

		[HttpPut("likes/unlike")]
		public async Task<ActionResult> UnlikeTweet([FromQuery] string at, [FromQuery] int tweetId) {
			User? user = await repo.GetUserAsync(at, false);
			if(user == null) return BadRequest();

			Tweet? tweet = await repo.GetTweetAsync(tweetId);
			if(tweet == null) return BadRequest();

			bool userHasAlreadyLiked = user.LikeHistory.Any(t => t.Id == tweet.Id);
			if(!userHasAlreadyLiked)
				return BadRequest();

			await repo.UnlikeTweetAsync(user, tweet);
			return NoContent();
		}

		[HttpDelete]
		public async Task<ActionResult> DeleteUser([FromQuery] string at) {
			var user = await repo.GetUserAsync(at, false);

			if(user == null) return BadRequest();

			await repo.DeleteUserAsync(user);
			return NoContent();
		}
	}
}