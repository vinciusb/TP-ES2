namespace TwitterAPI.Domain {
	public class User {
		public byte[] Color { get; private set; } = new byte[3];
		public string At { get; private set; }
		public string Username { get; private set; }
		public string Bio { get; private set; }
		public string City { get; private set; }
		public string Country { get; private set; }
		public IList<Tweet> Tweets { get; private set; }
		public IList<User> LikeHistory { get; private set; }
	}
}