using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterAPI.Application.Domain {
	public class Tweet {
		public int Id { get; set; }
		public User Owner { get; set; } = null!;
		public string Text { get; set; } = null!;
		public IList<Tweet> Replies { get; set; } = null!;
		public Tweet? ReplyTo { get; set; }
		public int Likes { get; set; }
		public DateTime PostTime { get; set; }
	}
}