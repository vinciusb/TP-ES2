using System.ComponentModel.DataAnnotations;

namespace TwitterAPI.Application.Domain {
	public class User {
		public int Id { get; set; }
		public string Color { get; set; } = null!;
		public string At { get; set; } = null!;
		public string Username { get; set; } = null!;
		public string Bio { get; set; } = null!;
		public string City { get; set; } = null!;
		public string Country { get; set; } = null!;
		public DateOnly? BirthDate { get; set; }
		public IList<Tweet> Tweets { get; set; } = null!;
		public IList<Tweet> LikeHistory { get; set; } = null!;
	}
}