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

		public bool Equals(User other) {
			if(other == null) return false;

			return Id == other.Id &&
				   Color == other.Color &&
				   At == other.At &&
				   Username == other.Username &&
				   Bio == other.Bio &&
				   City == other.City &&
				   Country == other.Country &&
				   BirthDate == other.BirthDate &&
				   Tweets == other.Tweets &&
				   LikeHistory == other.LikeHistory;
		}
	}
}