using Microsoft.AspNetCore.Mvc;
using TwitterAPI.Infrastructure.Persistence;
using TwitterAPI.Application.Utils.DTO;
using TwitterAPI.Application.Domain;
using TwitterAPI.Application.Utils;

namespace TwitterAPI.Presentation.Controllers {
	[ApiController]
	[Route("[controller]")]
	public class UserController : ControllerBase {
		private readonly ITwitterRepository repo;
		private readonly ILogger<UserController> _logger;

		public UserController(ITwitterRepository repo, ILogger<UserController> logger) {
			this.repo = repo;
			_logger = logger;
		}

		[HttpGet]
		public async Task<ActionResult> GetUsers() {
			throw new Exception("aaa");

		}

		[HttpGet("{at}")]
		public async Task<ActionResult> GetUser(string at) {
			throw new Exception("aaa");
		}

		[HttpPost]
		public async Task<ActionResult> PostUser([FromBody] UserDTO user) {
			User u = user.AsUser(new List<Tweet>(), new List<Tweet>());
			var userWasCreated = await repo.CreateUserAsync(u);

			if(userWasCreated) {
				// return CreatedAtAction(nameof(), new )
			}

			return Ok();
		}

		// [HttpPut]
		// public async Task<ActionResult> PostUser([FromBody] UserDTO user) { }

		// [HttpDelete]
		// public async Task<ActionResult> PostUser([FromBody] UserDTO user) { }
	}
}