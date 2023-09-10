using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterAPI.Domain {
	[NotMapped]
	public class Tweet {
		public int Id { get; set; }
		public User Owner { get; private set; }
		public string Text { get; private set; }
		public List<Tweet> Replies { get; private set; }
		public int Likes { get; private set; }
	}
}