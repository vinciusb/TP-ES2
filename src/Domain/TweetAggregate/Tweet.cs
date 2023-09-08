namespace TwitterAPI.Domain {
	public class Tweet {
		// private int Id { get; set; }
		public User Owner { get; private set; }
		public string Text { get; private set; }
		public List<Tweet> Replies { get; private set; }
		public int Likes { get; private set; }
	}
}