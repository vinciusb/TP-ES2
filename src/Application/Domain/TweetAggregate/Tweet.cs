using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterAPI.Application.Domain {
	public class Tweet {
		public int Id { get; set; }
		public User Owner { get; private set; } = null!;
		public string Text { get; private set; } = null!;
		public IList<Tweet> Replies { get; private set; } = null!;
		public Tweet? ReplyTo { get; private set; }
		public int Likes { get; private set; }
		public DateTime PostTime { get; private set; }
	}
}